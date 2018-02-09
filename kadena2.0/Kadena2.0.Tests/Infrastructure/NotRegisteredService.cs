using System;

namespace Kadena.Tests.Infrastructure
{
    public interface INotRegisteredSubservice
    {
        void Foo();
    }

    public class NotRegisteredService
    {
        private readonly INotRegisteredSubservice service;

        public NotRegisteredService(INotRegisteredSubservice service)
        {
            if(service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            this.service = service;
        }
    }
}
