namespace EventReplay
{
    using System;
    using System.Collections.Generic;
    using MediatR;

    public class UserNameToIdReadModel :
        INotificationHandler<UserCreatedEvent>,
        INotificationHandler<UsernameChangedEvent>
    {
        private static Dictionary<string, Guid> Readmodel = new Dictionary<string, Guid>();

        void INotificationHandler<UserCreatedEvent>.Handle(UserCreatedEvent notification)
        {
            Readmodel[notification.Username] = notification.Id;
        }

        void INotificationHandler<UsernameChangedEvent>.Handle(UsernameChangedEvent notification)
        {
            Readmodel[notification.Username] = notification.Id;
            Readmodel.Remove(notification.OldUsername);
        }

        public Guid GetIdByUserName(string username)
        {
            return Readmodel.ContainsKey(username) ? Readmodel[username] : Guid.Empty;
        }

    }
}
