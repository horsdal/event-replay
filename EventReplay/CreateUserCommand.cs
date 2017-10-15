namespace EventReplay
{
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
    private readonly AggregateRepository repo;
    private readonly EventDispatcher eventDispatcher;

    public CreateUserCommandHandler(AggregateRepository repo, EventDispatcher eventDispatcher)
    {
      this.repo = repo;
      this.eventDispatcher = eventDispatcher;
    }
        
    public Task Handle(CreateUserCommand command)
    {
      var aggregate = new UserAggregate();
      aggregate.Create(command);
      this.repo.Save(aggregate);
      return this.eventDispatcher.Dispatch(aggregate);
    }
  }

}