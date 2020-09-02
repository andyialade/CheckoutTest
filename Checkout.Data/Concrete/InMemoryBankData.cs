using Checkout.Domain.V1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Data
{
    public class InMemoryBankData : IBankData
    {
        List<ShopperCardModel> shopperCardModels;
        

        public InMemoryBankData()
        {
            shopperCardModels = new List<ShopperCardModel>()
            {
                new ShopperCardModel { ID = 1, FirstName = "Ibukun", LastName = "Alade", AmountAvailable = 250.00m, CardNumber = 4773273788896878, CurrencyCode = "GBP", CVV = 748, ExpiryDay = 5, ExpiryMonth = 2, Guid = new Guid ("2A9AC897E0F341B1BC42F3E381944EC3")  },
                new ShopperCardModel { ID = 2, FirstName = "Sam", LastName = "Channels", AmountAvailable = 150.00m, CardNumber = 4439325905458464, CurrencyCode = "GBP", CVV = 215, ExpiryDay = 15, ExpiryMonth = 10, Guid = new Guid ("5102C132883E4BB79C95B472F552E9F6")  }
            };
        }

        public IEnumerable<ShopperCardModel> GetAllShopperCardModels()
        {
            return shopperCardModels.OrderBy(x => x.FirstName);
        }

        public ShopperCardModel GetShopperCardModel(Guid guid)
        {
            return shopperCardModels.SingleOrDefault(x => x.Guid == guid);
        }

        public void ManageShopperAccount(Guid guid, decimal amount)
        {        
            var shopper = GetShopperCardModel(guid);  
            shopper.AmountAvailable -= amount;
        }
    }
}
