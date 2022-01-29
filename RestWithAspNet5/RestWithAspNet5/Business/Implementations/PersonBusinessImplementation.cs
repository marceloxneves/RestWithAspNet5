using RestWithAspNet5.Data.Converter.Implementations;
using RestWithAspNet5.Data.VO;
using RestWithAspNet5.Hypermedia.Utils;
using RestWithAspNet5.Model;
using RestWithAspNet5.Repository;
using RestWithAspNet5.Repository.Generic;
using System.Collections.Generic;

namespace RestWithAspNet5.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private IPersonRepository _repository;

        private readonly PersonConverter _converter;

        public PersonBusinessImplementation(IPersonRepository repository)
        {
            _repository = repository;

            _converter = new PersonConverter();
        }

        public PersonVO Create(PersonVO person)
        {
            var personEntity = _converter.Parse(person);

            return _converter.Parse(_repository.Create(personEntity));
        }

        public void Delete(long id)
        {
            _repository.Delete(id);           
        }

        public IEnumerable<PersonVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public PersonVO FindById(long id)
        {
            return _converter.Parse(_repository.FindById(id));
        }

        public IEnumerable<PersonVO> FindByName(string firstName, string lastName)
        {
            return _converter.Parse(_repository.FindByName(firstName, lastName));
        }

        public PersonVO Update(PersonVO person)
        {
            var personEntity = _converter.Parse(person);

            return _converter.Parse(_repository.Update(personEntity));
        }

        public PersonVO Disable(long id)
        {
            var personEntity = _repository.Disable(id);

            return _converter.Parse(personEntity);
        }

        public PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {            
            var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = pageSize < 1 ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            string query = @"select * from person p where 1 = 1 ";
            string countQuery = @"select count(*) from person p where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query + $" and p.first_name like '%{ name }%' ";
                countQuery = countQuery + $" and p.first_name like '%{ name }%' ";
            }

            query += $"order by p.first_name { sort } limit { size } offset { offset } ";
            
            var persons = _repository.FindWithPagedSearch(query);

            int totalResults = _repository.GetCount(countQuery);

            return new PagedSearchVO<PersonVO> 
            {
                CurrentPage = page,
                List = _converter.Parse(persons),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }
    }
}
