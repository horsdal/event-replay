namespace EventReplay
{
  using System;
  using EventReplay.Infrastructure;

  public class UsernameChangedEvent : Event<UserAggregate>
  {
    public string Username { get; }
    public Guid Id { get; }
    public string OldUsername { get; }

    public UsernameChangedEvent(string username, Guid id, string oldUsername)
    {
      this.Username = username;
      this.Id = id;
      this.OldUsername = oldUsername;
    }

    public void When(UserAggregate aggregate)
    {
      aggregate.Username = this.Username;
    }
  }
}