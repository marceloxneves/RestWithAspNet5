using RestWithAspNet5.Repository.Generic;
using RestWithASPNETUdemy.Model;
using System.Collections.Generic;

namespace RestWithASPNETUdemy.Business.Implementations
{
    public class BookBusinessImplementation : IBookBusiness
    {

        private readonly IRepository<Book> _repository;

        public BookBusinessImplementation(IRepository<Book> repository)
        {
            _repository = repository;
        }
        
        // Method responsible for returning all people,
        public IEnumerable<Book> FindAll()
        {
            return _repository.FindAll();
        }

        // Method responsible for returning one book by ID
        public Book FindByID(long id)
        {
            return _repository.FindById(id);
        }

        // Method responsible to crete one new book
        public Book Create(Book book)
        {
            return _repository.Create(book);
        }

        // Method responsible for updating one book
        public Book Update(Book book)
        {
            return _repository.Update(book);
        }

        // Method responsible for deleting a book from an ID
        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
