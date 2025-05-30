using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services;
using Moq;
using System.Linq.Expressions;

namespace HOA.Tests.Services
{
    public class PaymentsServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IPaymentsRepository> _mockPaymentsRepo;
        private readonly PaymentsService _service;
        private readonly List<Payment> _payments;

        public PaymentsServiceTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockPaymentsRepo = new Mock<IPaymentsRepository>();
            _mockRepo.Setup(r => r.PaymentsRepository).Returns(_mockPaymentsRepo.Object);
            _service = new PaymentsService(_mockRepo.Object);

            // Sample test data
            _payments = new List<Payment>
            {
                new Payment { Id = 1, Amount = 100, Apartment = 101, Status = "Pending" },
                new Payment { Id = 2, Amount = 200, Apartment = 102, Status = "Paid" },
            };
        }

        [Fact]
        public void GetPaymentsById_ExistingId_ReturnsPayment()
        {
            // Arrange
            var expected = _payments[0];
            _mockPaymentsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Payment, bool>>>()))
                .Returns(new List<Payment> { expected }.AsQueryable());

            // Act
            var result = _service.GetPaymentsById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
        }

        [Fact]
        public void AddPayment_ValidPayment_CallsCreateAndSave()
        {
            // Arrange
            var newPayment = new Payment { Amount = 300, Apartment = 103, Status = "Pending" };

            // Act
            _service.AddPayment(newPayment);

            // Assert
            _mockPaymentsRepo.Verify(r => r.Create(newPayment), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void UpdatePayment_ValidPayment_CallsUpdateAndSave()
        {
            // Arrange
            var updatedPayment = _payments[1];

            // Act
            _service.UpdatePayment(updatedPayment);

            // Assert
            _mockPaymentsRepo.Verify(r => r.Update(updatedPayment), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void DeletePayment_ExistingId_CallsDeleteAndSave()
        {
            // Arrange
            var paymentToDelete = _payments[0];
            _mockPaymentsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Payment, bool>>>()))
                .Returns(new List<Payment> { paymentToDelete }.AsQueryable());

            // Act
            _service.DeletePayment(paymentToDelete.Id);

            // Assert
            _mockPaymentsRepo.Verify(r => r.Delete(paymentToDelete), Times.Once);
            _mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void DeletePayment_NonExistingId_DoesNotCallDeleteOrSave()
        {
            // Arrange
            _mockPaymentsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Payment, bool>>>()))
                .Returns(Enumerable.Empty<Payment>().AsQueryable());

            // Act
            _service.DeletePayment(999);

            // Assert
            _mockPaymentsRepo.Verify(r => r.Delete(It.IsAny<Payment>()), Times.Never);
            _mockRepo.Verify(r => r.Save(), Times.Never);
        }

        [Fact]
        public void GetAllPayments_ReturnsAllPayments()
        {
            // Arrange
            _mockPaymentsRepo.Setup(r => r.FindAll()).Returns(_payments.AsQueryable());

            // Act
            var result = _service.GetAllPayments();

            // Assert
            Assert.Equal(_payments.Count, result.Count());
        }

        [Fact]
        public void SearchPaymentsByApartmentNumber_ValidNumber_ReturnsMatchingPayments()
        {
            // Arrange
            var query = "101";
            _mockPaymentsRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Payment, bool>>>()))
                .Returns(_payments.Where(p => p.Apartment == 101).AsQueryable());

            // Act
            var result = _service.SearchPaymentsByApartmentNumber(query);

            // Assert
            Assert.Single(result);
            Assert.All(result, p => Assert.Equal(101, p.Apartment));
        }

        [Fact]
        public void SearchPaymentsByApartmentNumber_InvalidNumber_ReturnsEmpty()
        {
            // Arrange
            var query = "205";

            // Act
            var result = _service.SearchPaymentsByApartmentNumber(query);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void UpdatePaymentStatus_ValidIdAndState_CallsUpdateStatus()
        {
            // Arrange
            var id = 1;
            var newState = "Paid";

            // Act
            _service.UpdatePaymentStatus(id, newState);

            // Assert
            _mockPaymentsRepo.Verify(r => r.UpdatePaymentStatus(id, newState), Times.Once);
        }
    }
}
