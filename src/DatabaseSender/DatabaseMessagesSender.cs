using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data.Configuration;
using Telerik.Sitefinity.LoadBalancing;
using Telerik.Sitefinity.LoadBalancing.Configuration;
using Telerik.Sitefinity.Services;

namespace DatabaseSender
{
    public class DatabaseMessagesSender : ISystemMessageSender
    {
        private const string _insertSql = "INSERT INTO [dbo].[_systemmessages] ([target],[data]) VALUES (@target, @data);";

        public void SendSystemMessage(SystemMessageBase msg)
        {
            string connectionString = Config.Get<DataConfig>().ConnectionStrings.ContainsKey("Sitefinity") ? Config.Get<DataConfig>().ConnectionStrings["Sitefinity"].ConnectionString : null;

            using (var connection = new SqlConnection(connectionString))
            {
                SystemMessageBase ourMessage = new SystemMessageBase
                {
                    Key = msg.Key,
                    MessageData = msg.MessageData
                };

                object messageToSerialize = (object)ourMessage;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    DataContractSerializer contractJsonSerializer = new DataContractSerializer(messageToSerialize.GetType(), (IEnumerable<Type>)new Type[1]
                    {
                        messageToSerialize.GetType()
                    });

                    try
                    {
                        contractJsonSerializer.WriteObject((Stream)memoryStream, messageToSerialize);
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                        throw;
                    }

                    ConfigManager manager = ConfigManager.GetManager();
                    SystemConfig section = manager.GetSection<SystemConfig>();
                    var urls = section.LoadBalancingConfig.URLS;
                    connection.Open();

                    foreach (var typeNameConfigElement in urls)
                    {
                        var command = new SqlCommand(_insertSql, connection);
                        command.Parameters.Add("@target", SqlDbType.VarChar);
                        command.Parameters["@target"].Value = typeNameConfigElement.Value;

                        command.Parameters.Add("@data", SqlDbType.VarBinary);
                        command.Parameters["@data"].Value = memoryStream.ToArray();

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("An excute non query exception. View the StackTrace:" + e.StackTrace);
                        }
                    }
                }
            }
        }

        public void SendSystemMessages(SystemMessageBase[] msgs)
        {
            foreach (SystemMessageBase systemMessage in msgs)
            {
                SendSystemMessage(systemMessage);
            }
        }

        public bool IsActive
        {
            get { return true; }
        }
    }
}
