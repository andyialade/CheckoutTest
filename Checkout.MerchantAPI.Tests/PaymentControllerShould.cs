using Checkout.Domain.V1;
using Checkout.Domain.V1.Models;
using Checkout.MerchantAPI.Controllers;
using Checkout.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Checkout.MerchantAPI.Tests
{
    public class PaymentControllerShould
    {
        [Fact]
        public void Return_BadRequest_No_MerchantCode_For_GetTransaction()
        {
            //Arrange
            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(false);

            var sut = new PaymentController(mockMerchantService.Object, null);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = string.Empty;

            //Act
            var result = sut.GetTransaction(0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Return_BadRequest_Invalid_MerchantCode_For_GetTransaction()
        {
            //Arrange
            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(false);

            var sut = new PaymentController(mockMerchantService.Object, null);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLEUR";

            //Act
            var result = sut.GetTransaction(0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Return_BadRequest_Invalid_TransactionId_For_GetTransaction()
        {
            //Arrange
            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(true);

            var sut = new PaymentController(mockMerchantService.Object, null);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLUK";

            //Act
            var result = sut.GetTransaction(0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Return_NotFound_With_TransactionId_That_Does_Not_Exist_For_GetTransaction()
        {
            //Arrange
            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(true);
            mockMerchantService.Setup(x => x.GetTransactionByID(It.IsAny<string>(), It.IsAny<int>())).Returns((TransactionModel)null);

            var sut = new PaymentController(mockMerchantService.Object, null);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLUK";

            //Act
            var result = sut.GetTransaction(1);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Return_InternalServerError_When_GetTransactionByID_Throws_Exception()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<PaymentController>>();

            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(true);
            mockMerchantService.Setup(x => x.GetTransactionByID(It.IsAny<string>(), It.IsAny<int>())).Throws<Exception>();

            var sut = new PaymentController(mockMerchantService.Object, mockLogger.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLUK";

            //Act
            var result = sut.GetTransaction(1);

            //Assert
            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public void Return_OK_With_Correct_TransactionId_For_GetTransaction()
        {
            //Arrange
            var transaction = new TransactionModel()
            {
                ID = 1,
                MaskCardNumber = "XXXXXXXXXXX-8464",
                CurrencyCode = "GBP",
                CVV = 215,
                ExpiryDay = 15,
                ExpiryMonth = 10,
                ShopperGuid = new Guid("5102C132883E4BB79C95B472F552E9F6"),
                Amount = 15.99m,
                MerchantCode = "APPLUK",
                CreatedDateTime = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.PaymentSuccessful
            };

            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(true);
            mockMerchantService.Setup(x => x.GetTransactionByID(It.IsAny<string>(), It.IsAny<int>())).Returns(transaction);

            var sut = new PaymentController(mockMerchantService.Object, null);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLUK";

            //Act
            var result = sut.GetTransaction(1);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Return_BadRequest_No_MerchantCode_For_ProcessPayment()
        {
            //Arrange
            var sut = new PaymentController(null, null);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = string.Empty;

            //Act
            var result = sut.ProcessPayment(It.IsAny<PaymentModel>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Return_BadRequest_Invalid_MerchantCode_For_ProcessPayment()
        {
            //Arrange
            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(false);

            var sut = new PaymentController(mockMerchantService.Object, null);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLUK";

            //Act
            var result = sut.ProcessPayment(It.IsAny<PaymentModel>());

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Return_BadRequest_Null_PaymentModel_For_ProcessPayment()
        {
            //Arrange
            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(true);

            var sut = new PaymentController(mockMerchantService.Object, null);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLUK";

            //Act
            var result = sut.ProcessPayment(null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Return_InternalServerError_When_TakePayment_Throw_Exception_For_ProcessPayment()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var mockLogger = new Mock<ILogger<PaymentController>>();

            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(true);
            mockMerchantService.Setup(x => x.TakePayment(It.IsAny<string>(), It.IsAny<PaymentModel>())).Throws<Exception>();

            var sut = new PaymentController(mockMerchantService.Object, mockLogger.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLUK";

            //Act
            var result = sut.ProcessPayment(paymentDetails);

            //Assert
            Assert.IsType<StatusCodeResult>(result);
        }

        [Fact]
        public void Return_BadRequest_When_TakePayment_Does_Not_Return_PaymentSuccessful_For_ProcessPayment()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var mockLogger = new Mock<ILogger<PaymentController>>();

            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(true);
            mockMerchantService.Setup(x => x.TakePayment(It.IsAny<string>(), It.IsAny<PaymentModel>())).Returns(PaymentStatus.Failed);

            var sut = new PaymentController(mockMerchantService.Object, mockLogger.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLUK";

            //Act
            var result = sut.ProcessPayment(paymentDetails);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Return_Ok_When_TakePayment_Returns_PaymentSuccessful_For_ProcessPayment()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var mockLogger = new Mock<ILogger<PaymentController>>();

            var mockMerchantService = new Mock<IMerchantService>();
            mockMerchantService.Setup(x => x.IsMerchantValid(It.IsAny<string>())).Returns(true);
            mockMerchantService.Setup(x => x.TakePayment(It.IsAny<string>(), It.IsAny<PaymentModel>())).Returns(PaymentStatus.PaymentSuccessful);

            var sut = new PaymentController(mockMerchantService.Object, mockLogger.Object);
            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Headers["merchantCode"] = "APPLUK";

            //Act
            var result = sut.ProcessPayment(paymentDetails);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

    }
}
