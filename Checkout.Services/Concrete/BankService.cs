using Checkout.Data;
using Checkout.Domain.V1;
using System;

namespace Checkout.Services
{
    public class BankService : IBankService
    {
        private readonly IBankData _bankData;

        public BankService(IBankData bankData)
        {
            _bankData = bankData;
        }

        public bool CanShopperPay(decimal amount, Guid guid)
        {
            var getShopperCardAllowance = _bankData.GetShopperCardModel(guid);
            return (getShopperCardAllowance != null && getShopperCardAllowance.AmountAvailable >= amount) ? true : false;
        }

        public bool UpdateShopperAccount(Guid guid, decimal amount)
        {
            try
            {
                _bankData.ManageShopperAccount(guid, amount);
                return true;
            }
            catch (Exception)
            {
                //throw;
                return false;
            }   
        }

        public bool ValidateCardDetails(PaymentModel userCardDetails)
        {
            var isCardValid = false;
            var userGuid = new Guid(userCardDetails.ShopperGuid);
            var getShopperCardDetails = _bankData.GetShopperCardModel(userGuid);

            if(getShopperCardDetails != null && getShopperCardDetails.CardNumber == userCardDetails.CardNumber && getShopperCardDetails.CVV == userCardDetails.CVV)
            {
                if (getShopperCardDetails.ExpiryMonth == userCardDetails.ExpiryMonth && getShopperCardDetails.ExpiryDay == userCardDetails.ExpiryDay)  
                {
                    if (getShopperCardDetails.CurrencyCode == userCardDetails.CurrencyCode.ToUpper())
                    {
                        isCardValid = true;
                    }
                }
            }

            return isCardValid;
        }
    }
}
