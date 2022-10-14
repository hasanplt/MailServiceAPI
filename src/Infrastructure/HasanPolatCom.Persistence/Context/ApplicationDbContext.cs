using HasanPolatCom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Persistence.Context
{
    public class ApplicationDbContext: DbContext
    {

        public DbSet<Mail> Mails { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions): base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                
            base.OnModelCreating(modelBuilder);
        }
    }
}
