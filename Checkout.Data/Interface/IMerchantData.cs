namespace Checkout.Data
{
    public interface IMerchantData
    {
        bool isMerchantValid(string code);
        void ProcessMerchantPayment(string code, decimal amount);
    }
}
