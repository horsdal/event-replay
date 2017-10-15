namespace EventPlayTests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using EventReplay;
    using EventReplay.Infrastructure;
    using MediatR;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;

    public class UnitTest1
    {
        private readonly UserNameToIdReadModel readmodel;
        private readonly AggregateRepository aggregateRepository;
        private readonly Mediator mediator;

        public UnitTest1()
        {
            this.mediator = new Mediator(SingleInstanceFactory, MultiInstanceFactory);
            this.aggregateRepository = new AggregateRepository();
            this.readmodel = new UserNameToIdReadModel();
        }

        private object SingleInstanceFactory(Type serviceType)
        {
            if (serviceType == typeof(IAsyncRequestHandler<CreateUserCommand>))
                return new CreateUserCommandHandler(new AggregateRepository(), new EventDispatcher(this.mediator));
            if (serviceType == typeof(INotificationHandler<UserCreatedEvent>))
                return new UserNameToIdReadModel();
            return null;
        }

        private IEnumerable<object> MultiInstanceFactory(Type serviceType)
        {
            if (serviceType == typeof(IAsyncRequestHandler<CreateUserCommand>))
                yield return new CreateUserCommandHandler(new AggregateRepository(), new EventDispatcher(this.mediator));
            if (serviceType == typeof(INotificationHandler<UserCreatedEvent>))
                yield return new UserNameToIdReadModel();
        }

        [Theory, AutoData]
        public async Task updates_readmodel(string email, string username)
        {
            var cmd = new CreateUserCommand(email, username);
            await this.mediator.Send(cmd);
            Assert.NotEqual(Guid.Empty, this.readmodel.GetIdByUserName(username));
        }
        
        [Theory, AutoData]
        public async Task saves_aggregate(string email, string username)
        {
            var cmd = new CreateUserCommand(email, username);
            await this.mediator.Send(cmd);
            var id = this.readmodel.GetIdByUserName(username);
            var actual = this.aggregateRepository.Get<UserAggregate>(id);
            Assert.NotNull(actual);
            Assert.Equal(id, actual.Id);
        }
    }
}
