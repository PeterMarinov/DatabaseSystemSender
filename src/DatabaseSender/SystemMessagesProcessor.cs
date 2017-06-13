using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data.Configuration;
using Telerik.Sitefinity.LoadBalancing;

namespace DatabaseSender
{
    public class SystemMessagesProcessor
    {
        #region Private Fields and Members

        private const string _selectSql = "SELECT [id], [target], [data] FROM [dbo].[_systemmessages] WHERE target=@target";
        private const string _deleteSql = "DELETE FROM [_systemmessages] WHERE id=@id";
        private List<SystemMessageBaseWrapper> _messages = new List<SystemMessageBaseWrapper>();

        #endregion

        #region Properties

        public List<SystemMessageBaseWrapper> Messages
        {
            get
            {
                return this._messages;
            }
        }

        #endregion

        #region Methods

        public void HandleSystemMessages()
        {
            string myself = Environment.MachineName;

            string connectionString = Config.Get<DataConfig>().ConnectionStrings.ContainsKey("Sitefinity") ? Config.Get<DataConfig>().ConnectionStrings["Sitefinity"].ConnectionString : null;

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(_selectSql, connection);
                command.Parameters.Add("target", SqlDbType.VarChar);
                command.Parameters["target"].Value = myself;
                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var result = (IDataRecord)reader;
                                var dataBytes = (byte[])reader["data"];
                                Guid id = reader.GetGuid(reader.GetOrdinal("id"));

                                SystemMessageBaseWrapper systemMessageBase = DeserializeSystemMessage(dataBytes, id);
                                _messages.Add(systemMessageBase);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }
                foreach (SystemMessageBaseWrapper systemMessage in _messages)
                {
                    try
                    {
                        SystemMessageDispatcher.HandleSystemMessage(systemMessage.SystemMessage);
                        CleanupMessage(connection, systemMessage);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                    }
                }
            }
        }

        private void CleanupMessage(SqlConnection connection, SystemMessageBaseWrapper systemMessage)
        {
            try
            {
                var deleteCommand = new SqlCommand(_deleteSql, connection);
                deleteCommand.Parameters.Add("id", SqlDbType.UniqueIdentifier);
                deleteCommand.Parameters["id"].Value = systemMessage.Id;

                deleteCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Bad exception: " + e.ToString());
            }
        }

        private SystemMessageBaseWrapper DeserializeSystemMessage(byte[] dataBytes, Guid id)
        {
            using (var ms = new MemoryStream(dataBytes))
            {
                ms.Position = 0;
                var contractJsonSerializer = new DataContractSerializer(new SystemMessageBase().GetType(), new Type[1]
                    {
                        new SystemMessageBase().GetType()
                    });

                try
                {
                    var message = (SystemMessageBase)contractJsonSerializer.ReadObject(ms);
                    var systemMessageBaseWrapper = new SystemMessageBaseWrapper(id, message);
                    return systemMessageBaseWrapper;
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to serialize: " + e.Message);
                    throw;
                }
            }
        }

        #endregion
    }
}
