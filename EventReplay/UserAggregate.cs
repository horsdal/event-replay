namespace EventReplay
{
  using System;
  using EventPlayTests;
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

    public void ChangeUsername(ChangeUsernameCommand command)
    {
      if (Guid.Empty.Equals(Id) || string.IsNullOrWhiteSpace(command.Username))
        return;

      var usernameChanged = new UsernameChangedEvent(command.Username, Id, this.username);
      Emit(usernameChanged);
    }

    protected void When(UserCreatedEvent e)
    {
      this.username = e.Username;
      this.email = e.EmailAddress;
      this.Id = e.Id;
    }

    protected void When(UsernameChangedEvent e)
    {
      this.username = e.Username;
    }
  }
}