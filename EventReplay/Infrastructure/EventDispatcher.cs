namespace EventReplay.Infrastructure
{
  using System.Linq;
  using System.Threading.Tasks;
  using MediatR;

  public class EventDispatcher
  {
    private readonly IMediator mediator;

    public EventDispatcher(IMediator mediator)
    {
      this.mediator = mediator;
    }
        
    public Task Dispatch<T>(Aggregate<T> aggregate) where T : Aggregate<T>
    {
      return Task.WhenAll(
        aggregate.NewEvents.Select(e => this.mediator.Publish<Event<T>>(e))
      );
    }
  }
}