using RestWithAspNet5.Hypermedia.Abstract;
using System.Collections.Generic;

namespace RestWithAspNet5.Hypermedia.Filters
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}
