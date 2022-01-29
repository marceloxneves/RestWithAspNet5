using System.Collections.Generic;

namespace RestWithAspNet5.Data.Converter.Contract
{
    public interface IParser<O, D>
    {
        D Parse(O origin);
        List<D> Parse(IEnumerable<O> origin);
    }
}
