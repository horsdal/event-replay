namespace EventReplay
{
  using System;
  using EventPlayTests;
  using EventReplay.Infrastructure;

  public class UserAggregate : Aggregate<UserAggregate>
  {
    public string Username { get; protected internal set; } = String.Empty;
    public string Email { get; internal set; } = String.Empty;
  }
}