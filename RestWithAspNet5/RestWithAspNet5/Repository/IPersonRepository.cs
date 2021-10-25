using RestWithAspNet5.Model;
using RestWithAspNet5.Repository.Generic;

namespace RestWithAspNet5.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
    }
}
