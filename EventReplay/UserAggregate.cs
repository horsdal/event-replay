namespace EventReplay
{
  using System;
  using EventPlayTests;
  using EventReplay.Infrastructure;

  public class UserAggregate : Aggregate
  {
    private readonly UserAggregateProjector projector;
    public string Username { get; internal set; } = String.Empty;
    public string Email { get; internal set; } = String.Empty;

    public UserAggregate()
    {
      this.projector = new UserAggregateProjector(this);
    }

    protected override object GetProjector()
    {
      return this.projector;
    }
  }

  public class UserAggregateProjector
  {
    private readonly UserAggregate projection;

    public UserAggregateProjector(UserAggregate projection)
    {
      this.projection = projection;
    }

    protected void When(UserCreatedEvent e)
    {
      this.projection.Username = e.Username;
      this.projection.Email = e.EmailAddress;
      this.projection.Id = e.Id;
    }

    protected void When(UsernameChangedEvent e)
    {
      this.projection.Username = e.Username;
    }
  }
}