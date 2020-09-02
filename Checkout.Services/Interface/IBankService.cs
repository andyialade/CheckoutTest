using Checkout.Domain.V1;
using System;

namespace Checkout.Services
{
    public interface IBankService
    {
        bool ValidateCardDetails(PaymentModel userCardDetails);

        bool CanShopperPay(decimal amount, Guid guid);

        bool UpdateShopperAccount(Guid guid, decimal amount);       
    }
}
