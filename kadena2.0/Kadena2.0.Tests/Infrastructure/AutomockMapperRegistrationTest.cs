using AutoMapper;
using Kadena.Dto.Product;
using Kadena.Models.Product;
using Kadena.Container.Default;
using Moq.AutoMock;
using System;
using Xunit;

namespace Kadena.Tests.Infrastructure
{
    [Trait("Infrastructure", "DIContainer")]
    public class AutomockMapperRegistrationTest
    {
        private class ServiceWithMapper
        {
            private readonly IMapper mapper;
            public ServiceWithMapper(IMapper mapper)
            {
                if (mapper == null)
                {
                    throw new ArgumentNullException(nameof(mapper));
                }

                this.mapper = mapper;
            }

            public BorderDto DoMapping(Border border)
            {
                return mapper.Map<BorderDto>(border);
            }
        }

        [Fact]
        public void TestRegisteredMapper()
        {
            // Arrange
            var autoMocker = new AutoMocker();
            autoMocker.Use<IMapper>(MapperBuilder.MapperInstance);
            var sut = autoMocker.CreateInstance<ServiceWithMapper>();

            // Act
            var result = sut.DoMapping(new Border { Exists = true, Value = "x" });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("x", result.Value);
            Assert.True(result.Exists);
        }

    }
}
