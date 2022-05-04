using Microsoft.EntityFrameworkCore;
using Muvi.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muvi.Data.Base
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly AppDbContext _context;
        private DbSet<T> genericT = null;

        public GenericService(AppDbContext context)
        {
            _context = context;
            genericT = _context.Set<T>();
                
        }

        public async Task Add(T entity)
        {
            genericT.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            T entityToDelete = genericT.Find(id);

            if ( _context.Entry(entityToDelete).State == EntityState.Detached)
            {
                genericT.Attach(entityToDelete);
            }
            
            genericT.Remove(entityToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            return await genericT.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll(string includeProperties)
        {
            IQueryable<T> query = genericT;

            foreach(var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public async Task Update(T newEntity)
        {
            genericT.Attach(newEntity);
            _context.Entry(newEntity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
