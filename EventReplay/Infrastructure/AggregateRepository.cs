namespace EventReplay.Infrastructure
{
  using System;
  using System.Collections.Generic;

  public class AggregateRepository<T> where T : Aggregate<T>
  {
    private static Dictionary<Guid, List<Event<T>>> eventStore = new Dictionary<Guid, List<Event<T>>>();
        
    public void Save(T aggregate)
    {
      if (!eventStore.ContainsKey(aggregate.Id))
        eventStore[aggregate.Id] = new List<Event<T>>();
      eventStore[aggregate.Id].AddRange(aggregate.NewEvents);
    }

    public T Get(Guid id)
    {
      if (!eventStore.ContainsKey(id))
        return default(T);
            
      var aggretate = Activator.CreateInstance<T>();
      aggretate.Replay(eventStore[id]);
      return aggretate;
    }
  }
}