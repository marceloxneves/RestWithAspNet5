using Microsoft.EntityFrameworkCore;
using RestWithAspNet5.Model.Context;
using RestWithASPNETUdemy.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithAspNet5.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected MySqlContext _context;
        private DbSet<T> dataset;

        public GenericRepository(MySqlContext context)
        {
            _context = context;
            dataset = _context.Set<T>();
        }

        public T Create(T entity)
        {
            try
            {
                dataset.Add(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }            
        }

        public void Delete(long id)
        {
            var result = dataset.Find(id);

            if (result != null)
            {
                dataset.Remove(result);
                _context.SaveChanges();
            }
        }

        public bool Exists(long id)
        {
            return dataset.Any(p => p.Id == id);
        }

        public IEnumerable<T> FindAll()
        {
            return dataset.ToList();
        }

        public T FindById(long id)
        {
            return dataset.Find(id);
        }

        public T Update(T entity)
        {
            if (!Exists(entity.Id))
            {
                return null;
            }

            var result = dataset.Find(entity.Id);

            try
            {
                _context.Entry(result).CurrentValues.SetValues(entity);
                _context.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }            
        }
    }
}
