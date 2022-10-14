using HasanPolatCom.Application.Interfaces.Repository;
using HasanPolatCom.Domain.Entities;
using HasanPolatCom.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> GetByAuth(string mail, string password)
        {
            return await dbContext.Users.Where(x => x.MailAddress == mail && x.AccountPassword == password).FirstOrDefaultAsync();
        }

        public async Task<User> GetByMail(string mail)
        {
            List<User> users = await dbContext.Users.ToListAsync();
            User user = await dbContext.Users.Where(x => x.MailAddress == mail).FirstOrDefaultAsync();
            return user;
        }
    }
}
