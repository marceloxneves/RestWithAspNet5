using RestWithAspNet5.Model;
using RestWithAspNet5.Model.Context;
using RestWithAspNet5.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestWithAspNet5.Repository
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(MySqlContext context) : base(context)
        {

        }

        public Person Disable(long id)
        {
            if (!_context.Persons.Any(p => p.Id == id)) return null;

            var user = _context.Persons.SingleOrDefault(p => p.Id.Equals(id));

            if(user != null)
            {
                user.Enabled = false;

                try
                {
                    _context.Entry(user).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return user;
        }

        public IEnumerable<Person> FindByName(string firstName, string lastName)
        {
            List<Person> persons = new List<Person>();

            if(!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                persons = _context.Persons.Where(p => p.FirstName.Contains(firstName) && p.LastName.Contains(lastName)).ToList();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    persons = _context.Persons.Where(p => p.FirstName.Contains(firstName)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    persons = _context.Persons.Where(p => p.LastName.Contains(lastName)).ToList();
                }                
            }

            if (persons != null && persons.Any())
            {
                return persons;
            }

            return null;
            

        }
    }
}
