using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using HOA.Controllers;
using HOA.Models;
using HOA.Services.Interfaces;
using HOA.Repositories.Interfaces;

namespace HOA.Tests.Controllers
{
    public class PaymentsControllerTests
    {
        private readonly Mock<IPaymentsService> _mockPaymentsService;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly PaymentsController _controller;

        public PaymentsControllerTests()
        {
            _mockPaymentsService = new Mock<IPaymentsService>();
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _controller = new PaymentsController(_mockPaymentsService.Object, _mockRepositoryWrapper.Object);
        }

        [Fact]
        public void PayNow_ReturnsViewWithPayment_WhenIdIsValid()
        {
            // Arrange
            var paymentId = 1;
            var payment = new Payment { Id = paymentId };
            _mockPaymentsService.Setup(service => service.GetPaymentsById(paymentId)).Returns(payment);

            // Act
            var result = _controller.PayNow(paymentId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Payment>(viewResult.Model);
            Assert.Equal(paymentId, model.Id);
        }

        [Fact]
        public void PayNow_ReturnsNotFound_WhenPaymentIsNull()
        {
            // Arrange
            var paymentId = 1;
            _mockPaymentsService.Setup(service => service.GetPaymentsById(paymentId)).Returns((Payment)null);

            // Act
            var result = _controller.PayNow(paymentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PayNowConfirmed_UpdatesPaymentAndRedirects_WhenPaymentExists()
        {
            // Arrange
            var paymentId = 1;
            var payment = new Payment { Id = paymentId };
            _mockPaymentsService.Setup(service => service.GetPaymentsById(paymentId)).Returns(payment);

            // Act
            var result = _controller.PayNowConfirmed(paymentId);

            // Assert
            _mockPaymentsService.Verify(service => service.UpdatePaymentStatus(paymentId, "Paid"), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void PayNowConfirmed_ReturnsNotFound_WhenPaymentDoesNotExist()
        {
            // Arrange
            var paymentId = 1;
            _mockPaymentsService.Setup(service => service.GetPaymentsById(paymentId)).Returns((Payment)null);

            // Act
            var result = _controller.PayNowConfirmed(paymentId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
