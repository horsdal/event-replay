namespace EventReplay.Infrastructure
{
  using System;
  using System.Threading.Tasks;

  public class CommandHandlersHelper<T>  where T : Aggregate<T>
  {
    private readonly AggregateRepository<T> repo;
    private readonly EventDispatcher eventDispatcher;

    public CommandHandlersHelper(AggregateRepository<T> repo, EventDispatcher eventDispatcher)
    {
      this.repo = repo;
      this.eventDispatcher = eventDispatcher;
    }

    public async Task Handle(Guid id, Func<T, T> handlerFunc)
    {
      var aggregate = this.repo.Get(id);
      aggregate = handlerFunc(aggregate);
      this.repo.Save(aggregate);
      await this.eventDispatcher.Dispatch(aggregate);
    }
    
    public async Task Handle(Guid id, Action<T> handlerFunc)
    {
      var aggregate = this.repo.Get(id);
      handlerFunc(aggregate);
      this.repo.Save(aggregate);
      await this.eventDispatcher.Dispatch(aggregate);
    }

  }
}