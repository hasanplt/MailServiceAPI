using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Application.Features.Commands.CreateMail
{
    public class MailRabbitMQDTO
    {
        public Guid Id { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
        public string Header { get; set; }

        public string SenderMail { get; set; }
        public string SenderPassword { get; set; }

        public bool IsSend { get; set; }

        public string Token { get; set; }
    }
}
