namespace Checkout.Domain.V1
{
    public class MerchantModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal CreditAmount { get; set; }
    }
}
