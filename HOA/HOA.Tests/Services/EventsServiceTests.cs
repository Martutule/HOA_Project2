using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services;
using Moq;
using System.Linq.Expressions;

namespace HOA.Tests.Services
{
    public class EventsServiceTests
    {
        private Mock<IRepositoryWrapper> _mockRepo;
        private Mock<IEventsRepository> _mockEventsRepo;
        private EventsService _service;
        private List<Event> _events;

        public EventsServiceTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockEventsRepo = new Mock<IEventsRepository>();
            _mockRepo.Setup(r => r.EventsRepository).Returns(_mockEventsRepo.Object);
            _service = new EventsService(_mockRepo.Object);

            // Sample test data
            _events = new List<Event>
            {
                new Event { Id = 1, EventName = "Summer Party", Date = DateTime.Now.AddDays(1) },
                new Event { Id = 2, EventName = "Winter Meeting", Date = DateTime.Now.AddDays(2) }
            };
        }

        [Fact]
        public void GetEventById_ExistingId_ReturnsEvent()
        {
            // Arrange
            var expectedEvent = _events[0];
            _mockEventsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(new List<Event> { expectedEvent }.AsQueryable());

            // Act
            var result = _service.GetEventById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedEvent.Id, result.Id);
        }

        [Fact]
        public void AddEvent_ValidEvent_CallsCreateAndSave()
        {
            // Arrange
            var newEvent = new Event { EventName = "New Event" };

            // Act
            _service.AddEvent(newEvent);

            // Assert
            _mockEventsRepo.Verify(r => r.Create(newEvent), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void UpdateEvent_ValidEvent_CallsUpdateAndSave()
        {
            // Arrange
            var eventToUpdate = _events[0];

            // Act
            _service.UpdateEvent(eventToUpdate);

            // Assert
            _mockEventsRepo.Verify(r => r.Update(eventToUpdate), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void DeleteEvent_ExistingId_CallsDeleteAndSave()
        {
            // Arrange
            var eventToDelete = _events[0];
            _mockEventsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(new List<Event> { eventToDelete }.AsQueryable());

            // Act
            _service.DeleteEvent(1);

            // Assert
            _mockEventsRepo.Verify(r => r.Delete(eventToDelete), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void GetAllEvents_ReturnsAllEvents()
        {
            // Arrange
            _mockEventsRepo.Setup(r => r.FindAll())
                .Returns(_events.AsQueryable());

            // Act
            var result = _service.GetAllEvents();

            // Assert
            Assert.Equal(_events.Count, result.Count());
        }

        [Fact]
        public void SearchEventsByEventName_ValidName_ReturnsMatchingEvents()
        {
            // Arrange
            var searchTerm = "Summer";
            _mockEventsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Event, bool>>>()))
                .Returns(_events.Where(e => e.EventName.ToLower().Contains(searchTerm.ToLower())).AsQueryable());

            // Act
            var result = _service.SearchEventsByEventName(searchTerm);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, e => e.EventName.Contains("Summer"));
        }

        [Fact]
        public void SortEventsByDate_Ascending_ReturnsSortedEvents()
        {
            // Arrange
            _mockEventsRepo.Setup(r => r.FindAll())
                .Returns(_events.AsQueryable());

            // Act
            var result = _service.SortEventsByDate(true).ToList();

            // Assert
            Assert.Equal(_events.OrderBy(e => e.Date).First().Id, result.First().Id);
        }

        [Fact]
        public void SortEventsByDate_Descending_ReturnsSortedEvents()
        {
            // Arrange
            _mockEventsRepo.Setup(r => r.FindAll())
                .Returns(_events.AsQueryable());

            // Act
            var result = _service.SortEventsByDate(false).ToList();

            // Assert
            Assert.Equal(_events.OrderByDescending(e => e.Date).First().Id, result.First().Id);
        }
    }
}