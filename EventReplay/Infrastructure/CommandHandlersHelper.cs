namespace EventReplay.Infrastructure
{
  using System;
  using System.Threading.Tasks;

  public class CommandHandlersHelper<T>  where T : Aggregate
  {
    private readonly AggregateRepository repo;
    private readonly EventDispatcher eventDispatcher;

    public CommandHandlersHelper(AggregateRepository repo, EventDispatcher eventDispatcher)
    {
      this.repo = repo;
      this.eventDispatcher = eventDispatcher;
    }

    public async Task Handle(Guid id, Func<T, T> handlerFunc)
    {
      var aggregate = this.repo.Get<T>(id);
      aggregate = handlerFunc(aggregate);
      this.repo.Save(aggregate);
      await this.eventDispatcher.Dispatch(aggregate);
    }
    
    public async Task Handle(Guid id, Action<T> handlerFunc)
    {
      var aggregate = this.repo.Get<T>(id);
      handlerFunc(aggregate);
      this.repo.Save(aggregate);
      await this.eventDispatcher.Dispatch(aggregate);
    }

  }
}