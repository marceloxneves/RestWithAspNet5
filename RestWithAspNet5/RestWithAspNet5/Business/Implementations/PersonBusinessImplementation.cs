using RestWithAspNet5.Data.Converter.Implementations;
using RestWithAspNet5.Data.VO;
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
    }
}
