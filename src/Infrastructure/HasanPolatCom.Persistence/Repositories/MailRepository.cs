using HasanPolatCom.Application.Interfaces.Repository;
using HasanPolatCom.Domain.Entities;
using HasanPolatCom.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Persistence.Repositories
{
    public class MailRepository: GenericRepository<Mail>, IMailRepository
    {

        private readonly ApplicationDbContext dbContext;

        public MailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Mail>> GetAllByReceiverAsync(string receiverMailAddress, string senderMailAddress)
        {
            var datas = await dbContext.Set<Mail>().Where(x => x.Receiver == receiverMailAddress && x.SenderMail == senderMailAddress).ToListAsync();

            return datas;
        }

        public async Task<List<Mail>> GetAllBySenderAsync(string mailAddress)
        {
            var datas = await dbContext.Set<Mail>().Where(x => x.SenderMail == mailAddress).ToListAsync();
            return datas;
        }

        public async Task<Mail> UpdateAsync(Mail mail, string senderMailAddress)
        {
            var data = await dbContext.Set<Mail>().Where(x => x.SenderMail == senderMailAddress && x.Id == mail.Id).FirstOrDefaultAsync();

            if (data == null)
                return null;

            return await UpdateAsync(mail);
        }
    }
}
