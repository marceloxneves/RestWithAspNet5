using RestWithAspNet5.Model;
using System.Collections.Generic;

namespace RestWithAspNet5.Repository
{
    public interface IPersonRepository
    {
        Person FindById(long id);
        IEnumerable<Person> FindAll();
        Person Create(Person person);
        Person Update(Person person);
        void Delete(long id);
        bool Exists(long id);
    }
}
