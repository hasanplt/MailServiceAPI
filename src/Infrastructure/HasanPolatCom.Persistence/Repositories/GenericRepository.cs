using HasanPolatCom.Application.Interfaces.Repository;
using HasanPolatCom.Domain.Common;
using HasanPolatCom.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanPolatCom.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepositoryAsync<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);

            await dbContext.SaveChangesAsync();
            
            return entity;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var datas = await dbContext.Set<T>().ToListAsync();
            return datas;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {

            var exist = dbContext.Set<T>()
                    .Where(i => i.Id == entity.Id).FirstOrDefault();

            dbContext.Entry(exist).CurrentValues.SetValues(entity);
            dbContext.SaveChanges();

            return entity;
        }
    }
}
