using HasanPolatCom.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Domain.Entities
{
    public class Mail: BaseEntity
    {
        public string Receiver { get; set; }
        public string Content { get; set; }
        public string Header { get; set; }
        public bool IsSend { get; set; } = false;
        public string SenderMail { get; set; }
        public string SenderPassword { get; set; }
    }
}
