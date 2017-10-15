namespace EventReplay
{
  using System;
  using EventReplay.Infrastructure;

  public class UserCreatedEvent : Event
  {
    public string Username { get; }
    public string EmailAddress { get; }
    public Guid Id { get; }

    public UserCreatedEvent(string username, string emailAddress, Guid id)
    {
      this.Username = username;
      this.EmailAddress = emailAddress;
      this.Id = id;
    }
  }
}