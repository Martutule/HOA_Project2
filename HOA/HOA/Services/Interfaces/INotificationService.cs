using HOA.Models;
using System.Collections.Generic;

namespace HOA.Services.Interfaces
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetAllNotifications();
        Notification GetNotificationById(int id);
        void CreateNotification(Notification notification);
        void UpdateNotification(Notification notification);
        void DeleteNotification(int id);
    }
}
