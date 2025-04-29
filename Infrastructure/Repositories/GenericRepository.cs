using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MainDBContext _dBContext;
        public GenericRepository(MainDBContext dBContext)
            => _dBContext = dBContext;
        public async Task Add(T entity)
        {
            await _dBContext.Set<T>().AddAsync(entity);
            await _dBContext.SaveChangesAsync();
        }
            
        public async Task Delete(T entity)
        {
            _dBContext.Set<T>().Remove(entity);
            await _dBContext.SaveChangesAsync();
        }
            
        public async Task<IReadOnlyList<T>> GetAll()
            =>  await _dBContext.Set<T>().ToListAsync();

        public async Task<T> GetById(int id)
            => await _dBContext.Set<T>().FindAsync(id);

        public async Task Update(T entity)
        {
             _dBContext.Set<T>().Update(entity);
             _dBContext.SaveChanges();
        }
           


        
    }
}
