using HasanPolatCom.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Domain.Entities
{
    public class User: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string AccountPassword { get; set; }
        public string MailAddress { get; set; }
        public string MailPassword { get; set; }
    }
}
