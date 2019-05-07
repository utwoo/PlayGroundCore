using AutoMapper;
using Console_Playground.Models.Destinations;
using Console_Playground.Models.Sources;

namespace Console_Playground.ValueResolvers
{
    public class SimpleValueResolver : IValueResolver<Simple, SimpleDTO, int>
    {
        public int Resolve(Simple source, SimpleDTO destination, int destMember, ResolutionContext context)
        {
            return source.Value1 + source.Value2;
        }
    }
}
