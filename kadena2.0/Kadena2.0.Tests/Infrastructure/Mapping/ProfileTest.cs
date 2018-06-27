using AutoMapper;

namespace Kadena.Tests.Infrastructure.Mapping
{
    public class ProfileTest<T> where T : Profile, new()
    {
        protected IMapper Sut => new MapperConfiguration(cfg => cfg.AddProfile<T>()).CreateMapper();
    }
}
