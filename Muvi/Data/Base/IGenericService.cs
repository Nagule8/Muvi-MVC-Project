using Muvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muvi.Data.Base
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(string includeProperties="");
        Task<T> Get(int id);
        Task Add(T obj);
        Task Update(T newObj);
        Task Delete(int id);
    }
}