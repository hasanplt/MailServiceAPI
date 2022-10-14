using HasanPolatCom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Application.Interfaces.Repository
{
    public interface IUserRepository: IGenericRepositoryAsync<User>
    {
        Task<User> GetByAuth(string mail, string password);
        Task<User> GetByMail(string mail);
    }
}
