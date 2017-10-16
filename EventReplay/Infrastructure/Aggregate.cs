namespace EventReplay.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  public abstract class Aggregate<T> where T : Aggregate<T>
  {
    private readonly List<Event<T>> newEvents = new List<Event<T>>();
    public Guid Id { get; protected internal set; }
    public IEnumerable<Event<T>> NewEvents => this.newEvents.AsReadOnly();

    public void Replay(List<Event<T>> events)
    {
      foreach (var @event in events)
        Play(@event);
    }
        
    internal void Emit(Event<T> @event)
    {
      this.newEvents.Add(@event);
      Play(@event);
    }

    private void Play(Event<T> @event)
    {
      @event.When(this as T);
    }
  }
}