using Moq.AutoMock;
using System;
using System.Linq.Expressions;

namespace Kadena.Tests
{
    public abstract class KadenaUnitTest<T> where T : class
    {
        private readonly AutoMocker autoMocker = new AutoMocker();

        protected T Sut => autoMocker.CreateInstance<T>();

        protected void Setup<TService, TResult>(Expression<Func<TService, TResult>> setupAction, TResult result) where TService : class
        {
            autoMocker.Setup(setupAction).Returns(result);
        }
    }
}
