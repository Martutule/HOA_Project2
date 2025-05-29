using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using HOA.Services;
using HOA.Services.Interfaces;
using HOA.Repositories.Interfaces;
using HOA.Models;

namespace HOA.Tests.Services
{
    public class DashboardServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IResidentsRepository> _mockResidentsRepository;
        private readonly Mock<IPaymentsRepository> _mockPaymentsRepository;
        private readonly Mock<IEventsRepository> _mockEventsRepository;
        private readonly Mock<IAnnouncementsRepository> _mockAnnouncementsRepository;
        private readonly Mock<INotificationRepository> _mockNotificationRepository;
        private readonly DashboardService _service;

        public DashboardServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockResidentsRepository = new Mock<IResidentsRepository>();
            _mockPaymentsRepository = new Mock<IPaymentsRepository>();
            _mockEventsRepository = new Mock<IEventsRepository>();
            _mockAnnouncementsRepository = new Mock<IAnnouncementsRepository>();
            _mockNotificationRepository = new Mock<INotificationRepository>();

            // Setup repository wrapper to return mocked repositories
            _mockRepositoryWrapper.Setup(r => r.ResidentsRepository).Returns(_mockResidentsRepository.Object);
            _mockRepositoryWrapper.Setup(r => r.PaymentsRepository).Returns(_mockPaymentsRepository.Object);
            _mockRepositoryWrapper.Setup(r => r.EventsRepository).Returns(_mockEventsRepository.Object);
            _mockRepositoryWrapper.Setup(r => r.AnnouncementsRepository).Returns(_mockAnnouncementsRepository.Object);
            _mockRepositoryWrapper.Setup(r => r.NotificationRepository).Returns(_mockNotificationRepository.Object);

            _service = new DashboardService(_mockRepositoryWrapper.Object);
        }


        [Fact]
        public void GetDashboardData_ReturnsZeroTotalResidents_WhenNoResidentsExist()
        {
            // Arrange
            var residents = new List<Resident>();
            _mockResidentsRepository.Setup(r => r.FindAll()).Returns(residents.AsQueryable());
            SetupDefaultMocks();

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.Equal(0, result.TotalResidents);
        }

        [Fact]
        public void GetDashboardData_ReturnsCorrectTotalDuePayments_WhenUnpaidPaymentsExist()
        {
            // Arrange
            var unpaidPayments = new List<Payment>
            {
                new Payment { Id = 1, Amount = 100.50f, Status = "Pending" },
                new Payment { Id = 2, Amount = 250.75f, Status = "Overdue" },
                new Payment { Id = 3, Amount = 50.25f, Status = "Pending" }
            };

            _mockPaymentsRepository.Setup(r => r.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Payment, bool>>>()))
                .Returns(unpaidPayments.AsQueryable());
            SetupDefaultMocksExceptPayments();

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.Equal(401.50f, result.TotalDuePayments);
        }

        [Fact]
        public void GetDashboardData_ReturnsZeroTotalDuePayments_WhenNoUnpaidPaymentsExist()
        {
            // Arrange
            var unpaidPayments = new List<Payment>();
            _mockPaymentsRepository.Setup(r => r.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Payment, bool>>>()))
                .Returns(unpaidPayments.AsQueryable());
            SetupDefaultMocksExceptPayments();

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.Equal(0f, result.TotalDuePayments);
        }

        [Fact]
        public void GetDashboardData_ReturnsCorrectTotalEvents_WhenEventsExist()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Id = 1, EventName = "Community Meeting" },
                new Event { Id = 2, EventName = "Pool Party" },
                new Event { Id = 3, EventName = "Maintenance Day" },
                new Event { Id = 4, EventName = "Holiday Celebration" }
            };

            _mockEventsRepository.Setup(r => r.FindAll()).Returns(events.AsQueryable());
            SetupDefaultMocksExceptEvents();

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.Equal(4, result.TotalEvents);
        }

        [Fact]
        public void GetDashboardData_ReturnsZeroTotalEvents_WhenNoEventsExist()
        {
            // Arrange
            var events = new List<Event>();
            _mockEventsRepository.Setup(r => r.FindAll()).Returns(events.AsQueryable());
            SetupDefaultMocksExceptEvents();

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.Equal(0, result.TotalEvents);
        }

        [Fact]
        public void GetDashboardData_ReturnsLast3Announcements_WhenAnnouncementsExist()
        {
            // Arrange
            var announcements = new List<Announcement>
            {
                new Announcement { Id = 1, Title = "First Announcement" },
                new Announcement { Id = 2, Title = "Second Announcement" },
                new Announcement { Id = 3, Title = "Third Announcement" },
                new Announcement { Id = 4, Title = "Fourth Announcement" },
                new Announcement { Id = 5, Title = "Fifth Announcement" }
            };

            _mockAnnouncementsRepository.Setup(r => r.FindAll()).Returns(announcements.AsQueryable());
            SetupDefaultMocksExceptAnnouncements();

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.Equal(3, result.RecentAnnouncements.Count);
            Assert.Equal("Fifth Announcement", result.RecentAnnouncements.First().Title);
            Assert.Equal("Fourth Announcement", result.RecentAnnouncements.Skip(1).First().Title);
            Assert.Equal("Third Announcement", result.RecentAnnouncements.Skip(2).First().Title);
        }

        [Fact]
        public void GetDashboardData_ReturnsEmptyAnnouncementsList_WhenNoAnnouncementsExist()
        {
            // Arrange
            var announcements = new List<Announcement>();
            _mockAnnouncementsRepository.Setup(r => r.FindAll()).Returns(announcements.AsQueryable());
            SetupDefaultMocksExceptAnnouncements();

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.Empty(result.RecentAnnouncements);
        }

        [Fact]
        public void GetDashboardData_ReturnsTop5Notifications_WhenNotificationsExist()
        {
            // Arrange
            var notifications = new List<Notification>
            {
                new Notification { Id = 1, Message = "First Notification" },
                new Notification { Id = 2, Message = "Second Notification" },
                new Notification { Id = 3, Message = "Third Notification" },
                new Notification { Id = 4, Message = "Fourth Notification" },
                new Notification { Id = 5, Message = "Fifth Notification" },
                new Notification { Id = 6, Message = "Sixth Notification" },
                new Notification { Id = 7, Message = "Seventh Notification" }
            };

            _mockNotificationRepository.Setup(r => r.FindAll()).Returns(notifications.AsQueryable());
            SetupDefaultMocksExceptNotifications();

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.Equal(5, result.Notifications.Count);
            Assert.Equal("Seventh Notification", result.Notifications.First().Message);
            Assert.Equal("Sixth Notification", result.Notifications.Skip(1).First().Message);
            Assert.Equal("Fifth Notification", result.Notifications.Skip(2).First().Message);
            Assert.Equal("Fourth Notification", result.Notifications.Skip(3).First().Message);
            Assert.Equal("Third Notification", result.Notifications.Skip(4).First().Message);
        }

        [Fact]
        public void GetDashboardData_ReturnsEmptyNotificationsList_WhenNoNotificationsExist()
        {
            // Arrange
            var notifications = new List<Notification>();
            _mockNotificationRepository.Setup(r => r.FindAll()).Returns(notifications.AsQueryable());
            SetupDefaultMocksExceptNotifications();

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.Empty(result.Notifications);
        }

        [Fact]
        public void GetDashboardData_ReturnsCompleteData_WhenAllDataExists()
        {
            // Arrange
            var residents = new List<Resident> { new Resident { Id = 1 } };
            var unpaidPayments = new List<Payment> { new Payment { Id = 1, Amount = 100f, Status = "Pending" } };
            var events = new List<Event> { new Event { Id = 1 } };
            var announcements = new List<Announcement> { new Announcement { Id = 1, Title = "Test" } };
            var notifications = new List<Notification> { new Notification { Id = 1, Message = "Test" } };

            _mockResidentsRepository.Setup(r => r.FindAll()).Returns(residents.AsQueryable());
            _mockPaymentsRepository.Setup(r => r.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Payment, bool>>>()))
                .Returns(unpaidPayments.AsQueryable());
            _mockEventsRepository.Setup(r => r.FindAll()).Returns(events.AsQueryable());
            _mockAnnouncementsRepository.Setup(r => r.FindAll()).Returns(announcements.AsQueryable());
            _mockNotificationRepository.Setup(r => r.FindAll()).Returns(notifications.AsQueryable());

            // Act
            var result = _service.GetDashboardData();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalResidents);
            Assert.Equal(100f, result.TotalDuePayments);
            Assert.Equal(1, result.TotalEvents);
            Assert.Single(result.RecentAnnouncements);
            Assert.Single(result.Notifications);
        }

        private void SetupDefaultMocks()
        {
            _mockResidentsRepository.Setup(r => r.FindAll()).Returns(new List<Resident>().AsQueryable());
            _mockPaymentsRepository.Setup(r => r.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Payment, bool>>>()))
                .Returns(new List<Payment>().AsQueryable());
            _mockEventsRepository.Setup(r => r.FindAll()).Returns(new List<Event>().AsQueryable());
            _mockAnnouncementsRepository.Setup(r => r.FindAll()).Returns(new List<Announcement>().AsQueryable());
            _mockNotificationRepository.Setup(r => r.FindAll()).Returns(new List<Notification>().AsQueryable());
        }

        private void SetupDefaultMocksExceptPayments()
        {
            _mockResidentsRepository.Setup(r => r.FindAll()).Returns(new List<Resident>().AsQueryable());
            _mockEventsRepository.Setup(r => r.FindAll()).Returns(new List<Event>().AsQueryable());
            _mockAnnouncementsRepository.Setup(r => r.FindAll()).Returns(new List<Announcement>().AsQueryable());
            _mockNotificationRepository.Setup(r => r.FindAll()).Returns(new List<Notification>().AsQueryable());
        }

        private void SetupDefaultMocksExceptEvents()
        {
            _mockResidentsRepository.Setup(r => r.FindAll()).Returns(new List<Resident>().AsQueryable());
            _mockPaymentsRepository.Setup(r => r.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Payment, bool>>>()))
                .Returns(new List<Payment>().AsQueryable());
            _mockAnnouncementsRepository.Setup(r => r.FindAll()).Returns(new List<Announcement>().AsQueryable());
            _mockNotificationRepository.Setup(r => r.FindAll()).Returns(new List<Notification>().AsQueryable());
        }

        private void SetupDefaultMocksExceptAnnouncements()
        {
            _mockResidentsRepository.Setup(r => r.FindAll()).Returns(new List<Resident>().AsQueryable());
            _mockPaymentsRepository.Setup(r => r.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Payment, bool>>>()))
                .Returns(new List<Payment>().AsQueryable());
            _mockEventsRepository.Setup(r => r.FindAll()).Returns(new List<Event>().AsQueryable());
            _mockNotificationRepository.Setup(r => r.FindAll()).Returns(new List<Notification>().AsQueryable());
        }

        private void SetupDefaultMocksExceptNotifications()
        {
            _mockResidentsRepository.Setup(r => r.FindAll()).Returns(new List<Resident>().AsQueryable());
            _mockPaymentsRepository.Setup(r => r.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Payment, bool>>>()))
                .Returns(new List<Payment>().AsQueryable());
            _mockEventsRepository.Setup(r => r.FindAll()).Returns(new List<Event>().AsQueryable());
            _mockAnnouncementsRepository.Setup(r => r.FindAll()).Returns(new List<Announcement>().AsQueryable());
        }
    }
}