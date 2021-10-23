using RestWithAspNet5.Data.VO;
using RestWithASPNETUdemy.Model;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);
        BookVO FindByID(long id);
        IEnumerable<BookVO> FindAll();
        BookVO Update(BookVO book);
        void Delete(long id);
    }
}
