using Checkout.Domain.V1;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Data
{
    public class MerchantData : IMerchantData
    {
        List<MerchantModel> merchantModels;

        public MerchantData()
        {
            merchantModels = new List<MerchantModel>()
            {
                new MerchantModel {  ID  =  1, Name = "Amazon", CreditAmount = 0.0m, Code = "AMAUK"},
                new MerchantModel {  ID  =  2, Name = "Apple", CreditAmount = 0.0m, Code = "APPLUK"}
            };
        }

        public bool isMerchantValid(string code)
        {
            var merchant = merchantModels.SingleOrDefault(x => x.Code.ToUpper() == code.ToUpper());
            var result = merchant != null ? true : false;
            return result;
        }

        public void ProcessMerchantPayment(string code, decimal amount)
        {
            var merchant = merchantModels.SingleOrDefault(x => x.Code == code);
            merchant.CreditAmount += amount;
        }
    }
}
