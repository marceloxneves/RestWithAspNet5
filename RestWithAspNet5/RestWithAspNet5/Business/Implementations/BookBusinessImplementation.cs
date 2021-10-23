using RestWithAspNet5.Data.Converter.Implementations;
using RestWithAspNet5.Data.VO;
using RestWithAspNet5.Repository.Generic;
using RestWithASPNETUdemy.Model;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Business.Implementations
{
    public class BookBusinessImplementation : IBookBusiness
    {

        private readonly IRepository<Book> _repository;

        private readonly BookConverter _converter;

        public BookBusinessImplementation(IRepository<Book> repository)
        {
            _repository = repository;

            _converter = new BookConverter();
        }
        
        // Method responsible for returning all people,
        public IEnumerable<BookVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        // Method responsible for returning one book by ID
        public BookVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindById(id));
        }

        // Method responsible to crete one new book
        public BookVO Create(BookVO book)
        {
            var bookEntity = _converter.Parse(book);

            return _converter.Parse(_repository.Create(bookEntity));
        }

        // Method responsible for updating one book
        public BookVO Update(BookVO book)
        {
            var bookEntity = _converter.Parse(book);

            return _converter.Parse(_repository.Update(bookEntity));
        }

        // Method responsible for deleting a book from an ID
        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
