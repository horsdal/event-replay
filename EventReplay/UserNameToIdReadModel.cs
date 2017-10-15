namespace EventReplay
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using MediatR;

    public class UserNameToIdReadModel : INotificationHandler<UserCreatedEvent>
    {
        private static Dictionary<string, Guid> Readmodel = new Dictionary<string, Guid>();

        void INotificationHandler<UserCreatedEvent>.Handle(UserCreatedEvent notification)
        {
            Readmodel[notification.Username] = notification.Id;
        }

        public Guid GetIdByUserName(string username)
        {
            return Readmodel.ContainsKey(username) ? Readmodel[username] : Guid.Empty;
        }
    }
}
