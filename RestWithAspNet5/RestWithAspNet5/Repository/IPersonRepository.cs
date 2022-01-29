using RestWithAspNet5.Model;
using RestWithAspNet5.Repository.Generic;
using System.Collections.Generic;

namespace RestWithAspNet5.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
        IEnumerable<Person> FindByName(string firstName, string lastName);
    }
}
