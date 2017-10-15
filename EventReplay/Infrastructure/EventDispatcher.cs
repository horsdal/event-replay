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
        
    public Task Dispatch(Aggregate aggregate)
    {
      return Task.WhenAll(
        aggregate.NewEvents.Select(e => this.mediator.Publish<Event>(e))
      );
    }
  }
}