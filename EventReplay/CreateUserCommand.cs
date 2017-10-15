namespace EventReplay
{
  using System;
  using System.Threading.Tasks;
  using EventReplay.Infrastructure;
  using MediatR;

  public class CreateUserCommand : IRequest
  {
    public string Username { get; }
    public string EmailAddress { get; }

    public CreateUserCommand(string emailAddress, string username)
    {
      this.EmailAddress = emailAddress;
      this.Username = username;
    }
  }
  
  public class CreateUserCommandHandler : IAsyncRequestHandler<CreateUserCommand>
  {
    private readonly CommandHandlersHelper<UserAggregate> helper;

    public CreateUserCommandHandler(CommandHandlersHelper<UserAggregate> helper)
    {
      this.helper = helper;
    }
        
    public Task Handle(CreateUserCommand command)
    {
      return this.helper.Handle(Guid.Empty, _ =>
      {
        var aggregate = new UserAggregate();
        var createdEvent = new UserCreatedEvent(command.Username, command.EmailAddress, Guid.NewGuid());
        aggregate.Emit(createdEvent);
        return aggregate;
      });
    }
  }

}