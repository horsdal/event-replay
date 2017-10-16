namespace EventReplay.Infrastructure
{
  using MediatR;

  public interface Event<T> : INotification where T : Aggregate<T>
  {
    void When(T aggregate);
  }
}