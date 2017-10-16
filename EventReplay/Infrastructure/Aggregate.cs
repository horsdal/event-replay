namespace EventReplay.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  public abstract class Aggregate
  {
    private readonly List<Event> newEvents = new List<Event>();
    public Guid Id { get; internal set; }
    public IEnumerable<Event> NewEvents => this.newEvents.AsReadOnly();

    public void Replay(List<Event> events)
    {
      foreach (var @event in events)
        Play(@event);
    }
        
    internal void Emit(Event @event)
    {
      this.newEvents.Add(@event);
      Play(@event);
    }

    private void Play(Event @event)
    {
      var projector = this.GetProjector();
      var whenMethod = projector
        .GetType()
        .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(m => m.Name.Equals("When"))
        .Where(m => m.GetParameters().SingleOrDefault(p => p.ParameterType.FullName.Equals(@event.GetType().FullName)) != null);
      whenMethod.Single().Invoke(projector,  new object[] { @event });
    }

    protected abstract object GetProjector();
  }
}