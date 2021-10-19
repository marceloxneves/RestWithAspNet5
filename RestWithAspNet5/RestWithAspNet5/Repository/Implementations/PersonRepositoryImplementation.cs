using RestWithAspNet5.Model;
using RestWithAspNet5.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWithAspNet5.Repository.Implementations
{
    public class PersonRepositoryImplementation : IPersonRepository
    {
        private MySqlContext _context;

        public PersonRepositoryImplementation(MySqlContext context)
        {
            _context = context;
        }

        public Person Create(Person person)
        {
            try
            {
                _context.Add(person);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return person;
        }

        public void Delete(long id)
        {
            var result = _context.Persons.Find(id);

            if(result != null)
            {
                _context.Remove(result);
                _context.SaveChanges();
            }            
        }

        public IEnumerable<Person> FindAll()
        {
            return _context.Persons.ToList();
        }

        public Person FindById(long id)
        {
            return _context.Persons.Find(id);
        }

        public Person Update(Person person)
        {
            if(!Exists(person.Id))
            {
                return null;
            }

            var result = _context.Persons.Find(person.Id);

            try
            {
                _context.Entry(result).CurrentValues.SetValues(person);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return person;
        }

        public bool Exists(long id)
        {
            return _context.Persons.Any(p => p.Id == id);
        }
    }
}
