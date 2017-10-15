namespace EventReplay.Infrastructure
{
  using System;
  using System.Collections.Generic;

  public class AggregateRepository
  {
    private static Dictionary<Guid, List<Event>> eventStore = new Dictionary<Guid, List<Event>>();
        
    public void Save(Aggregate aggregate)
    {
      if (!eventStore.ContainsKey(aggregate.Id))
        eventStore[aggregate.Id] = new List<Event>();
      eventStore[aggregate.Id].AddRange(aggregate.NewEvents);
    }

    public Aggregate Get<T>(Guid id) where T : Aggregate
    {
      if (!eventStore.ContainsKey(id))
        return null;
            
      var aggretate = Activator.CreateInstance<T>();
      aggretate.Replay(eventStore[id]);
      return aggretate;
    }
  }
}