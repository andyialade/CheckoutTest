namespace Checkout.Domain.V1
{
    public class PaymentModel
    {
        public long CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryDay { get; set; }
        public string CurrencyCode { get; set; }
        public int CVV { get; set; } //Card Security Code
        public decimal Amount { get; set; }
        public string ShopperGuid { get; set; }
    }
}
