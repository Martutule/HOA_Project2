using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace HOA.Tests.Services
{
    public class IncidentsServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IIncidentsRepository> _mockIncidentsRepo;
        private readonly IncidentsService _service;
        private readonly List<Incident> _incidents;

        public IncidentsServiceTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockIncidentsRepo = new Mock<IIncidentsRepository>();
            _mockRepo.Setup(r => r.IncidentsRepository).Returns(_mockIncidentsRepo.Object);
            _service = new IncidentsService(_mockRepo.Object);

            // Sample test data
            _incidents = new List<Incident>
            {
                new Incident { Id = 1, Title = "Water Leak", Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)), Status = "Open" },
                new Incident { Id = 2, Title = "Broken Window", Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), Status = "Closed" },
            };
        }

        [Fact]
        public void GetIncidentsById_ExistingId_ReturnsIncident()
        {
            // Arrange
            var expected = _incidents[0];
            _mockIncidentsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Incident, bool>>>()))
                .Returns(new List<Incident> { expected }.AsQueryable());

            // Act
            var result = _service.GetIncidentsById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
        }

        [Fact]
        public void AddIncident_ValidIncident_CallsCreateAndSave()
        {
            // Arrange
            var newIncident = new Incident { Title = "Power Outage" };

            // Act
            _service.AddIncident(newIncident);

            // Assert
            _mockIncidentsRepo.Verify(r => r.Create(newIncident), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void UpdateIncident_ValidIncident_CallsUpdateAndSave()
        {
            // Arrange
            var incidentToUpdate = _incidents[1];

            // Act
            _service.UpdateIncident(incidentToUpdate);

            // Assert
            _mockIncidentsRepo.Verify(r => r.Update(incidentToUpdate), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void DeleteIncident_ExistingId_CallsDeleteAndSave()
        {
            // Arrange
            var incidentToDelete = _incidents[0];
            _mockIncidentsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Incident, bool>>>()))
                .Returns(new List<Incident> { incidentToDelete }.AsQueryable());

            // Act
            _service.DeleteIncident(incidentToDelete.Id);

            // Assert
            _mockIncidentsRepo.Verify(r => r.Delete(incidentToDelete), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void DeleteIncident_NonExistingId_DoesNotCallDeleteOrSave()
        {
            // Arrange
            _mockIncidentsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Incident, bool>>>()))
                .Returns(Enumerable.Empty<Incident>().AsQueryable());

            // Act
            _service.DeleteIncident(999);

            // Assert
            _mockIncidentsRepo.Verify(r => r.Delete(It.IsAny<Incident>()), Times.Never);
            _mockRepo.Verify(r => r.Save(), Times.Never);
        }

        [Fact]
        public void GetAllIncidents_ReturnsAllIncidents()
        {
            // Arrange
            _mockIncidentsRepo.Setup(r => r.FindAll()).Returns(_incidents.AsQueryable());

            // Act
            var result = _service.GetAllIncidents();

            // Assert
            Assert.Equal(_incidents.Count, result.Count());
        }

        [Fact]
        public void SearchIncidentsByIncidentName_ValidName_ReturnsMatchingIncidents()
        {
            // Arrange
            var name = "leak";
            _mockIncidentsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Incident, bool>>>()))
                .Returns(_incidents.Where(i => i.Title.ToLower().Contains(name.ToLower())).AsQueryable());

            // Act
            var result = _service.SearchIncidentsByIncidentName(name);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, i => i.Title.ToLower().Contains(name));
        }

        [Fact]
        public void UpdateIncidentStatus_ValidIdAndStatus_CallsUpdateStatus()
        {
            // Arrange
            var id = 1;
            var status = "Resolved";

            // Act
            _service.UpdateIncidentStatus(id, status);

            // Assert
            _mockIncidentsRepo.Verify(r => r.UpdateIncidentStatus(id, status), Times.Once);
        }

        [Fact]
        public void SortIncidentsByDate_Descending_ReturnsDescendingList()
        {
            // Arrange
            _mockIncidentsRepo.Setup(r => r.FindAll()).Returns(_incidents.AsQueryable());

            // Act
            var result = _service.SortIncidentsByDate(true).ToList();

            // Assert
            Assert.Equal(_incidents.OrderByDescending(i => i.Date).First().Id, result.First().Id);
        }

        [Fact]
        public void SortIncidentsByDate_Ascending_ReturnsAscendingList()
        {
            // Arrange
            _mockIncidentsRepo.Setup(r => r.FindAll()).Returns(_incidents.AsQueryable());

            // Act
            var result = _service.SortIncidentsByDate(false).ToList();

            // Assert
            Assert.Equal(_incidents.OrderBy(i => i.Date).First().Id, result.First().Id);
        }
    }
}
