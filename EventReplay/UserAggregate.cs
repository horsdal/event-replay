namespace EventReplay
{
  using System;
  using EventPlayTests;
  using EventReplay.Infrastructure;

  public class UserAggregate : Aggregate
  {
    public string Username { get; private set; } = String.Empty;
    public string Email { get; private set; } = String.Empty;

    protected void When(UserCreatedEvent e)
    {
      this.Username = e.Username;
      this.Email = e.EmailAddress;
      this.Id = e.Id;
    }

    protected void When(UsernameChangedEvent e)
    {
      this.Username = e.Username;
    }
  }
}