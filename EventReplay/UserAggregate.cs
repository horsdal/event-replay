namespace EventReplay
{
  using System;
  using EventReplay.Infrastructure;

  public class UserAggregate : Aggregate
  {
    private string username = String.Empty; 
    private string email = String.Empty;

    public void Create(CreateUserCommand command)
    {
      var createdEvent = new UserCreatedEvent(command.Username, command.EmailAddress, Guid.NewGuid());
      Emit(createdEvent);
    }

    protected void When(UserCreatedEvent e)
    {
      this.username = e.Username;
      this.email = e.EmailAddress;
      this.Id = e.Id;
    }
  }
}