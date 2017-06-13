using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.LoadBalancing;

namespace DatabaseSender
{
    public class SystemMessageBaseWrapper
    {
        public SystemMessageBaseWrapper(Guid id, SystemMessageBase systemMessage)
        {
            this.id = id;
            this.systemMessage = systemMessage;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public SystemMessageBase SystemMessage
        {
            get
            {
                return this.systemMessage;
            }
        }

        private Guid id;
        private SystemMessageBase systemMessage;
    }
}
