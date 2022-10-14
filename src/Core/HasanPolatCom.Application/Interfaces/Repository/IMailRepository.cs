using HasanPolatCom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Application.Interfaces.Repository
{
    public interface IMailRepository: IGenericRepositoryAsync<Mail>
    {
        Task<List<Mail>> GetAllByReceiverAsync(string receiverMailAddress, string senderMailAddress);
        Task<List<Mail>> GetAllBySenderAsync(string mailAddress);
        Task<Mail> UpdateAsync(Mail mail, string senderMailAddress);
    }
}
