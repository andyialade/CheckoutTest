using Checkout.Domain.V1;
using System;
using System.Collections.Generic;

namespace Checkout.Data
{
    public interface IBankData
    {
        IEnumerable<ShopperCardModel> GetAllShopperCardModels();
        ShopperCardModel GetShopperCardModel(Guid guid);
        void ManageShopperAccount(Guid guid, decimal amount);
    }
}
