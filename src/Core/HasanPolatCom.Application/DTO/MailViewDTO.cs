using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Application.DTO
{
    public class MailViewDTO
    {
        public string Receiver { get; set; }
        public string Content { get; set; }
        public string Header { get; set; }
        public bool IsSend { get; set; }
    }
}
