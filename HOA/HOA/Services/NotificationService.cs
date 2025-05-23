using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services.Interfaces;
using Mono.TextTemplating;
using System.Collections.Generic;
using System.Linq;

namespace HOA.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public NotificationService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        // Get all notifications ordered by Timestamp descending
        public IEnumerable<Notification> GetAllNotifications()
        {
            return _repositoryWrapper.NotificationRepository.FindAll()
                .OrderByDescending(n => n.Timestamp)
                .ToList();
        }

        
        public Notification GetNotificationById(int id)
        {
            // You can either throw NotImplementedException or remove this method
            throw new System.NotImplementedException("GetNotificationById is not supported because Notification has no Id property.");
        }

        // Create a new notification
        public void CreateNotification(Notification notification)
        {
            _repositoryWrapper.NotificationRepository.Create(notification);
            _repositoryWrapper.Save();
        }

        // Update an existing notification 
        public void UpdateNotification(Notification notification)
        {
            _repositoryWrapper.NotificationRepository.Update(notification);
            _repositoryWrapper.Save();
        }

        // Delete a notification by id 
        public void DeleteNotification(int id)
        {
            var notification = _repositoryWrapper.NotificationRepository.FindByCondition(n => n.Id == id).FirstOrDefault();

            if (notification != null)
            {
                _repositoryWrapper.NotificationRepository.Delete(notification);
                _repositoryWrapper.Save();
            }
        }
    }
}
