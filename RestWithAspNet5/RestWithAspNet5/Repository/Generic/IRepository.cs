using RestWithASPNETUdemy.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithAspNet5.Repository.Generic
{
    public interface IRepository<T> where T : BaseEntity
    {
        T FindById(long id);
        IEnumerable<T> FindAll();
        T Create(T entity);
        T Update(T entity);
        void Delete(long id);
        bool Exists(long id);
    }
}
