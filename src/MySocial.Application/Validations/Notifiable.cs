namespace MySocial.Application.Validations
{
    public class Notifiable
    {
        private readonly List<Notification> _notifications;

        public Notifiable()
        {
            _notifications = new List<Notification>();
        }

        public IReadOnlyCollection<Notification> Notifications => _notifications;

        public void AddNotification(string message) => _notifications.Add(new Notification(message));

        public bool HasNotifications => _notifications.Any();
    }
}
