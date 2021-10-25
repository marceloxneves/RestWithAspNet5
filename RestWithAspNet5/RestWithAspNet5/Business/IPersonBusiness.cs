using RestWithAspNet5.Data.VO;
using RestWithAspNet5.Model;
using System.Collections.Generic;

namespace RestWithAspNet5.Business
{
    public interface IPersonBusiness
    {
        PersonVO FindById(long id);
        IEnumerable<PersonVO> FindAll();
        PersonVO Create(PersonVO person);
        PersonVO Update(PersonVO person);
        PersonVO Disable(long id);
        void Delete(long id);
    }
}
