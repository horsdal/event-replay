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
        private readonly AggregateRepository<UserAggregate> aggregateRepository;
        private readonly Mediator mediator;

        public UnitTest1()
        {
            this.mediator = new Mediator(SingleInstanceFactory, MultiInstanceFactory);
            this.aggregateRepository = new AggregateRepository<UserAggregate>();
            this.readmodel = new UserNameToIdReadModel();
        }

        private object SingleInstanceFactory(Type serviceType)
        {
            if (serviceType == typeof(IAsyncRequestHandler<CreateUserCommand>))
                return new CreateUserCommandHandler(new CommandHandlersHelper<UserAggregate>(new AggregateRepository<UserAggregate>(), new EventDispatcher(this.mediator)));
            if (serviceType == typeof(IAsyncRequestHandler<ChangeUsernameCommand>))
                return new ChangeUsernameCommandHandler(new CommandHandlersHelper<UserAggregate>(new AggregateRepository<UserAggregate>(), new EventDispatcher(this.mediator)));
            if (serviceType == typeof(INotificationHandler<UserCreatedEvent>)
             || serviceType == typeof(INotificationHandler<UsernameChangedEvent>))
                return new UserNameToIdReadModel();
            return null;
        }

        private IEnumerable<object> MultiInstanceFactory(Type serviceType)
        {
            var instance = SingleInstanceFactory(serviceType);
            if (instance != null)
                yield return instance;
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
            var actual = this.aggregateRepository.Get(id);
            Assert.NotNull(actual);
            Assert.Equal(id, actual.Id);
        }

        [Theory, AutoData]
        public async Task change_username(string email, string username, string username2)
        {
            var createUser = new CreateUserCommand(email, username);
            await this.mediator.Send(createUser);
            var id = this.readmodel.GetIdByUserName(username);

            var changeUsername = new ChangeUsernameCommand(id, username2);
            await this.mediator.Send(changeUsername);

            Assert.Equal(id, this.readmodel.GetIdByUserName(username2));
        }
    }
}
