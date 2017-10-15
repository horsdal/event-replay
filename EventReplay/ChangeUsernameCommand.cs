namespace EventPlayTests
{
  using System;
  using System.Threading.Tasks;
  using EventReplay;
  using EventReplay.Infrastructure;
  using MediatR;

  public class ChangeUsernameCommand : IRequest
  {
    public Guid Id { get; }
    public string Username { get; }

    public ChangeUsernameCommand(Guid id, string username)
    {
      this.Id = id;
      this.Username = username;
    }
  }
  
  public class ChangeUsernameCommandHandler : IAsyncRequestHandler<ChangeUsernameCommand>
  {
    private readonly CommandHandlersHelper<UserAggregate> helper;

    public ChangeUsernameCommandHandler(CommandHandlersHelper<UserAggregate> helper)
    {
      this.helper = helper;
    }
    
    public Task Handle(ChangeUsernameCommand command)
    {
      return this.helper.Handle(command.Id, aggregate =>
      {
        if (Guid.Empty.Equals(aggregate.Id) || string.IsNullOrWhiteSpace(command.Username))
          return;
        var usernameChanged = new UsernameChangedEvent(command.Username, aggregate.Id, aggregate.Username);
        aggregate.Emit(usernameChanged);
      });
    }
  }
}