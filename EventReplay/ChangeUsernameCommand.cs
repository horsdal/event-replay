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
    private readonly AggregateRepository repo;
    private readonly EventDispatcher eventDispatcher;

    public ChangeUsernameCommandHandler(AggregateRepository repo, EventDispatcher eventDispatcher)
    {
      this.repo = repo;
      this.eventDispatcher = eventDispatcher;
    }
    public Task Handle(ChangeUsernameCommand command)
    {
      var aggregate = this.repo.Get<UserAggregate>(command.Id);
      aggregate.ChangeUsername(command);
      this.repo.Save(aggregate);
      return this.eventDispatcher.Dispatch(aggregate);
    }
  }
}