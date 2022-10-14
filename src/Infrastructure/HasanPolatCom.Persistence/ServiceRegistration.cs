using HasanPolatCom.Application.Interfaces.Repository;
using HasanPolatCom.Persistence.Context;
using HasanPolatCom.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Persistence
{
    public static class ServiceRegistration
    {

        public static void AddPersistenceServices(this IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("memoryDb"));

            services.AddTransient<IMailRepository, MailRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

        }

    }
}
