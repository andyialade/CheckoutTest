using System;

namespace Checkout.Domain.V1.Models
{
    public class TransactionModel
    {
        public int ID { get; set; }
        public string MaskCardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryDay { get; set; }
        public string CurrencyCode { get; set; }
        public int CVV { get; set; } //Card Security Code
        public decimal Amount { get; set; }
        public string MerchantCode { get; set; }
        public Guid ShopperGuid { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
