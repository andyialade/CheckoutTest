using Checkout.Data;
using Checkout.Domain.V1;
using Checkout.Domain.V1.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Checkout.Services.Tests
{
    public class MerchantServiceShould
    {
        [Fact]
        public void Return_False_Invalid_MerchantCode()
        {
            //Arrange
            var mockMerchantData = new Mock<IMerchantData>();
            mockMerchantData.Setup(x => x.isMerchantValid(It.IsAny<string>())).Returns(false);

            var sut = new MerchantService(null, null, mockMerchantData.Object);

            //Act
            var result = sut.IsMerchantValid(It.IsAny<string>());

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_True_Valid_MerchantCode()
        {
            //Arrange
            var mockMerchantData = new Mock<IMerchantData>();
            mockMerchantData.Setup(x => x.isMerchantValid(It.IsAny<string>())).Returns(true);

            var sut = new MerchantService(null, null, mockMerchantData.Object);

            //Act
            var result = sut.IsMerchantValid(It.IsAny<string>());

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Return_Null_With_GetTransactionByID()
        {
            //Arrange
            var mockTransactionData = new Mock<ITransactionData>();
            mockTransactionData.Setup(x => x.GetTransactionById(It.IsAny<string>(), It.IsAny<int>())).Returns((TransactionModel)null);

            var sut = new MerchantService(null, mockTransactionData.Object, null);

            //Act
            var result = sut.GetTransactionByID(It.IsAny<string>(), It.IsAny<int>());

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void Return_Transaction_With_GetTransactionByID()
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

            var mockTransactionData = new Mock<ITransactionData>();
            mockTransactionData.Setup(x => x.GetTransactionById("APPLUK", 1)).Returns(transaction);

            var sut = new MerchantService(null, mockTransactionData.Object, null);

            //Act
            var result = sut.GetTransactionByID("APPLUK", 1);

            //Assert
            Assert.Equal(transaction,result);
        }

        [Fact]
        public void Return_Transactions_With_GetAllTransactions()
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

            var appleTransactions = new List<TransactionModel>() { transaction };


            var mockTransactionData = new Mock<ITransactionData>();
            mockTransactionData.Setup(x => x.GetAllTransactions("APPLUK")).Returns(appleTransactions);

            var sut = new MerchantService(null, mockTransactionData.Object, null);

            //Act
            var result = sut.GetAllTransactions("APPLUK");

            //Assert
            Assert.Single(result);
        }


        [Fact]
        public void Return_FailedCardVerification_TakePayment()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                Amount = 10.00m,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var mockBankService = new Mock<IBankService>();
            mockBankService.Setup(x => x.ValidateCardDetails(paymentDetails)).Returns(false);
            //mockBankService.Setup(x => x.CanShopperPay(paymentDetails.Amount, new Guid(paymentDetails.ShopperGuid))).Returns(true);
            //mockBankService.Setup(x => x.UpdateShopperAccount(new Guid(paymentDetails.ShopperGuid), paymentDetails.Amount)).Returns(true);

            //var mockMerchantData = new Mock<IMerchantData>();
            //mockMerchantData.Setup(x => x.ProcessMerchantPayment(It.IsAny<string>(), paymentDetails.Amount)).Verifiable();

            var mockTransactionData = new Mock<ITransactionData>();
            mockTransactionData.Setup(x => x.AddTransaction(It.IsAny<TransactionModel>())).Verifiable();

            var sut = new MerchantService(mockBankService.Object, mockTransactionData.Object, null);

            //Act
            var result = sut.TakePayment("APPLUK", paymentDetails);

            //Assert
            Assert.Equal(PaymentStatus.FailedCardVerification, result);
        }

        [Fact]
        public void Return_InsufficientFunds_TakePayment()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                Amount = 10.00m,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var mockBankService = new Mock<IBankService>();
            mockBankService.Setup(x => x.ValidateCardDetails(paymentDetails)).Returns(true);
            mockBankService.Setup(x => x.CanShopperPay(paymentDetails.Amount, new Guid(paymentDetails.ShopperGuid))).Returns(false);
            //mockBankService.Setup(x => x.UpdateShopperAccount(new Guid(paymentDetails.ShopperGuid), paymentDetails.Amount)).Returns(true);

            //var mockMerchantData = new Mock<IMerchantData>();
            //mockMerchantData.Setup(x => x.ProcessMerchantPayment(It.IsAny<string>(), paymentDetails.Amount)).Verifiable();

            var mockTransactionData = new Mock<ITransactionData>();
            mockTransactionData.Setup(x => x.AddTransaction(It.IsAny<TransactionModel>())).Verifiable();

            var sut = new MerchantService(mockBankService.Object, mockTransactionData.Object, null);

            //Act
            var result = sut.TakePayment("APPLUK", paymentDetails);

            //Assert
            Assert.Equal(PaymentStatus.InsufficientFunds, result);
        }

        [Fact]
        public void Return_Failed_TakePayment()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                Amount = 10.00m,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var mockBankService = new Mock<IBankService>();
            mockBankService.Setup(x => x.ValidateCardDetails(paymentDetails)).Returns(true);
            mockBankService.Setup(x => x.CanShopperPay(paymentDetails.Amount, new Guid(paymentDetails.ShopperGuid))).Returns(true);
            //mockBankService.Setup(x => x.UpdateShopperAccount(new Guid(paymentDetails.ShopperGuid), paymentDetails.Amount)).Returns(true);

            var mockMerchantData = new Mock<IMerchantData>();
            mockMerchantData.Setup(x => x.ProcessMerchantPayment(It.IsAny<string>(), paymentDetails.Amount)).Throws<Exception>();

            var mockTransactionData = new Mock<ITransactionData>();
            mockTransactionData.Setup(x => x.AddTransaction(It.IsAny<TransactionModel>())).Verifiable();

            var sut = new MerchantService(mockBankService.Object, mockTransactionData.Object, mockMerchantData.Object);

            //Act
            var result = sut.TakePayment("APPLUK", paymentDetails);

            //Assert
            Assert.Equal(PaymentStatus.Failed, result);
        }

        [Fact]
        public void Return_Failed_UpdateShopperAccount_TakePayment()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                Amount = 10.00m,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var mockBankService = new Mock<IBankService>();
            mockBankService.Setup(x => x.ValidateCardDetails(paymentDetails)).Returns(true);
            mockBankService.Setup(x => x.CanShopperPay(paymentDetails.Amount, new Guid(paymentDetails.ShopperGuid))).Returns(true);
            mockBankService.Setup(x => x.UpdateShopperAccount(new Guid(paymentDetails.ShopperGuid), paymentDetails.Amount)).Returns(false);

            var mockMerchantData = new Mock<IMerchantData>();
            mockMerchantData.Setup(x => x.ProcessMerchantPayment(It.IsAny<string>(), paymentDetails.Amount)).Verifiable();

            var mockTransactionData = new Mock<ITransactionData>();
            mockTransactionData.Setup(x => x.AddTransaction(It.IsAny<TransactionModel>())).Verifiable();

            var sut = new MerchantService(mockBankService.Object, mockTransactionData.Object, mockMerchantData.Object);

            //Act
            var result = sut.TakePayment("APPLUK", paymentDetails);

            //Assert
            Assert.Equal(PaymentStatus.Failed, result);
        }

        [Fact]
        public void Return_PaymentSuccessful_TakePayment()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                Amount = 10.00m,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var mockBankService = new Mock<IBankService>();
            mockBankService.Setup(x => x.ValidateCardDetails(paymentDetails)).Returns(true);
            mockBankService.Setup(x => x.CanShopperPay(paymentDetails.Amount, new Guid(paymentDetails.ShopperGuid))).Returns(true);
            mockBankService.Setup(x => x.UpdateShopperAccount(new Guid(paymentDetails.ShopperGuid), paymentDetails.Amount)).Returns(true);

            var mockMerchantData = new Mock<IMerchantData>();
            mockMerchantData.Setup(x => x.ProcessMerchantPayment(It.IsAny<string>(), paymentDetails.Amount)).Verifiable();

            var mockTransactionData = new Mock<ITransactionData>();
            mockTransactionData.Setup(x => x.AddTransaction(It.IsAny<TransactionModel>())).Verifiable();

            var sut = new MerchantService(mockBankService.Object, mockTransactionData.Object, mockMerchantData.Object);

            //Act
            var result = sut.TakePayment("APPLUK", paymentDetails);

            //Assert
            Assert.Equal(PaymentStatus.PaymentSuccessful, result);
        }
    }
}
