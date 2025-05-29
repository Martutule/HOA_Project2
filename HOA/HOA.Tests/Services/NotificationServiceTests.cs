using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services;
using Moq;
using System.Linq.Expressions;

namespace HOA.Tests
{
    public class NotificationServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<INotificationRepository> _mockNotificationRepo;
        private readonly NotificationService _service;
        private readonly List<Notification> _notifications;

        public NotificationServiceTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockNotificationRepo = new Mock<INotificationRepository>();
            _mockRepo.Setup(r => r.NotificationRepository).Returns(_mockNotificationRepo.Object);
            _service = new NotificationService(_mockRepo.Object);

            // Sample test data
            _notifications = new List<Notification>
            {
                new Notification { Id = 1, Message = "Test 1", Timestamp = DateTime.Now.AddMinutes(-10) },
                new Notification { Id = 2, Message = "Test 2", Timestamp = DateTime.Now.AddMinutes(-5) },
                new Notification { Id = 3, Message = "Test 3", Timestamp = DateTime.Now.AddMinutes(-1) }
            };
        }

        [Fact]
        public void GetAllNotifications_ReturnsNotificationsInDescendingOrder()
        {
            // Arrange
            _mockNotificationRepo.Setup(r => r.FindAll()).Returns(_notifications.AsQueryable());

            // Act
            var result = _service.GetAllNotifications().ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(_notifications.OrderByDescending(n => n.Timestamp).First().Id, result.First().Id);
        }

        [Fact]
        public void CreateNotification_ValidNotification_CallsCreateAndSave()
        {
            // Arrange
            var newNotification = new Notification { Message = "New notification", Timestamp = DateTime.Now };

            // Act
            _service.CreateNotification(newNotification);

            // Assert
            _mockNotificationRepo.Verify(r => r.Create(newNotification), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void UpdateNotification_ValidNotification_CallsUpdateAndSave()
        {
            // Arrange
            var notificationToUpdate = _notifications[0];

            // Act
            _service.UpdateNotification(notificationToUpdate);

            // Assert
            _mockNotificationRepo.Verify(r => r.Update(notificationToUpdate), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void DeleteNotification_ExistingId_CallsDeleteAndSave()
        {
            // Arrange
            var notificationToDelete = _notifications[1];
            _mockNotificationRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Notification, bool>>>()))
                .Returns(new List<Notification> { notificationToDelete }.AsQueryable());

            // Act
            _service.DeleteNotification(notificationToDelete.Id);

            // Assert
            _mockNotificationRepo.Verify(r => r.Delete(notificationToDelete), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void DeleteNotification_NonExistingId_DoesNotCallDeleteOrSave()
        {
            // Arrange
            _mockNotificationRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Notification, bool>>>()))
                .Returns(Enumerable.Empty<Notification>().AsQueryable());

            // Act
            _service.DeleteNotification(999);

            // Assert
            _mockNotificationRepo.Verify(r => r.Delete(It.IsAny<Notification>()), Times.Never);
            _mockRepo.Verify(r => r.Save(), Times.Never);
        }
    }
}
