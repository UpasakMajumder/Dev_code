using Moq;
using Moq.AutoMock;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kadena.Tests
{
    public abstract class KadenaUnitTest<TSut> : IEnumerable<object[]>
        where TSut : class
    {
        private readonly AutoMocker autoMocker = new AutoMocker();

        public IEnumerator<object[]> GetEnumerator()
        {
            var dependencies = typeof(TSut)
                .GetConstructors()
                .Select(c => c.GetParameters())
                .Where(p => p.Length != 0)
                .FirstOrDefault()?
                .Select(p =>
                {
                    var mockType = typeof(Mock<>).MakeGenericType(p.ParameterType);
                    return ((Mock)Activator.CreateInstance(mockType)).Object;
                })
                .ToArray()
                ?? new object[0];

            foreach (var dep in dependencies)
            {
                yield return dependencies
                    .Select(d => d.Equals(dep) ? null : d)
                    .ToArray();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected TSut Sut => autoMocker.CreateInstance<TSut>();

        protected void Setup<TService, TResult>(Expression<Func<TService, TResult>> setupAction, TResult result) where TService : class
        {
            autoMocker.Setup(setupAction).Returns(result);
        }

        protected void Setup<TService, TArg, TResult>(Expression<Func<TService, TResult>> setupAction, Func<TArg, TResult> result) where TService : class
        {
            autoMocker.Setup(setupAction).Returns(result);
        }

        protected void Setup<TService, TArg1, TArg2, TResult>(Expression<Func<TService, TResult>> setupAction, Func<TArg1, TArg2, TResult> result) where TService : class
        {
            autoMocker.Setup(setupAction).Returns(result);
        }

        protected void SetupThrows<TService>(Expression<Action<TService>> setupAction, Exception exception) where TService : class
        {
            autoMocker.Setup<TService>(setupAction).Throws(exception);
        }

        protected void Verify<TService>(Expression<Action<TService>> verifyAction, Func<Times> times) where TService : class
        {
            autoMocker.Verify(verifyAction, times);
        }

        protected void Verify<TService>(Expression<Action<TService>> verifyAction, Times times) where TService : class
        {
            autoMocker.Verify(verifyAction, times);
        }

        protected void VerifyNoOtherCalls<TService>() where TService : class
        {
            autoMocker.GetMock<TService>().VerifyNoOtherCalls();
        }

        protected void Use<TMock, TService>()
            where TMock : class
            where TService : class, TMock
        {
            Use<TMock>(autoMocker.CreateInstance<TService>());
        }

        protected void Use<TService>(TService service) where TService : class
        {
            autoMocker.Use(service);
        }
    }
}
