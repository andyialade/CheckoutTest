using System;

namespace Checkout.Domain.V1
{
    public class ShopperCardModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryDay { get; set; }
        public string CurrencyCode { get; set; }
        public int CVV { get; set; } //Card Security Code
        public decimal AmountAvailable { get; set; }
        public Guid Guid { get; set; }
    }
}
