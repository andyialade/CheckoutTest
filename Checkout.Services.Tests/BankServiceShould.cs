using Checkout.Data;
using Checkout.Domain.V1;
using Moq;
using System;
using Xunit;

namespace Checkout.Services.Tests
{
    public class BankServiceShould
    {
        [Fact]
        public void Return_False_For_CanShopperPay()
        {
            //Arrange
            var cardDetails = new ShopperCardModel()
            {
               ID = 1, FirstName = "John", LastName = "Doe", AmountAvailable = 50.00m, CardNumber = 4773273788896878, 
               CurrencyCode = "GBP", CVV = 748, ExpiryDay = 5, ExpiryMonth = 2, Guid = new Guid ("2A9AC897E0F341B1BC42F3E381944EC3") 
            };

            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns(cardDetails);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.CanShopperPay(100.00m, Guid.NewGuid());

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_False_For_CanShopperPay_With_Null_CardDetails()
        {
            //Arrange
            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns((ShopperCardModel)null);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.CanShopperPay(100.00m, Guid.NewGuid());

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_True_For_CanShopperPay()
        {
            //Arrange
            var cardDetails = new ShopperCardModel()
            {
                ID = 1,
                FirstName = "John",
                LastName = "Doe",
                AmountAvailable = 100.00m,
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 748,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                Guid = new Guid("2A9AC897E0F341B1BC42F3E381944EC3")
            };

            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns(cardDetails);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.CanShopperPay(100.00m, Guid.NewGuid());

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Return_True_For_UpdateShopperAccount()
        {
            //Arrange
            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.ManageShopperAccount(It.IsAny<Guid>(), It.IsAny<decimal>()));

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.UpdateShopperAccount(Guid.NewGuid(), 100.00m);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Return_False_For_UpdateShopperAccount_With_Exception()
        {
            //Arrange
            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.ManageShopperAccount(It.IsAny<Guid>(), It.IsAny<decimal>())).Throws<Exception>();

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.UpdateShopperAccount(Guid.NewGuid(), 100.00m);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_False_For_ValidateCardDetails_Null_CardDetails()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns((ShopperCardModel)null);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.ValidateCardDetails(paymentDetails);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_False_For_ValidateCardDetails_Invalid_CardNumber()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var cardDetails = new ShopperCardModel()
            {
                CardNumber = 4773273775812878,
                Guid = new Guid("2A9AC897E0F341B1BC42F3E381944EC3")
            };


            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns(cardDetails);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.ValidateCardDetails(paymentDetails);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_False_For_ValidateCardDetails_Invalid_CVV()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CVV = 748,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var cardDetails = new ShopperCardModel()
            {
                CardNumber = 4773273788896878,
                CVV = 123,
                Guid = new Guid("2A9AC897E0F341B1BC42F3E381944EC3")
            };


            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns(cardDetails);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.ValidateCardDetails(paymentDetails);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_False_For_ValidateCardDetails_Invalid_ExpiryMonth()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CVV = 123,
                ExpiryMonth = 2,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var cardDetails = new ShopperCardModel()
            {
                CardNumber = 4773273788896878,
                CVV = 123,
                ExpiryMonth = 1,
                Guid = new Guid("2A9AC897E0F341B1BC42F3E381944EC3")
            };


            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns(cardDetails);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.ValidateCardDetails(paymentDetails);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_False_For_ValidateCardDetails_Invalid_ExpiryDay()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CVV = 123,
                ExpiryDay = 10,
                ExpiryMonth = 2,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var cardDetails = new ShopperCardModel()
            {
                CardNumber = 4773273788896878,
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                Guid = new Guid("2A9AC897E0F341B1BC42F3E381944EC3")
            };


            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns(cardDetails);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.ValidateCardDetails(paymentDetails);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_False_For_ValidateCardDetails_Invalid_Currency()
        {
            //Arrange
            var paymentDetails = new PaymentModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 10,
                ExpiryMonth = 2,
                ShopperGuid = "2A9AC897E0F341B1BC42F3E381944EC3"
            };

            var cardDetails = new ShopperCardModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "USD",
                CVV = 123,
                ExpiryDay = 10,
                ExpiryMonth = 2,
                Guid = new Guid("2A9AC897E0F341B1BC42F3E381944EC3")
            };


            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns(cardDetails);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.ValidateCardDetails(paymentDetails);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Return_True_For_ValidateCardDetails_Correct_CardDetails()
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

            var cardDetails = new ShopperCardModel()
            {
                CardNumber = 4773273788896878,
                CurrencyCode = "GBP",
                CVV = 123,
                ExpiryDay = 5,
                ExpiryMonth = 2,
                Guid = new Guid("2A9AC897E0F341B1BC42F3E381944EC3")
            };


            var mockBankData = new Mock<IBankData>();
            mockBankData.Setup(x => x.GetShopperCardModel(It.IsAny<Guid>())).Returns(cardDetails);

            var sut = new BankService(mockBankData.Object);

            //Act
            var result = sut.ValidateCardDetails(paymentDetails);

            //Assert
            Assert.True(result);
        }
    }
}
