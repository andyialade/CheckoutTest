using Checkout.Domain.V1;
using Checkout.Domain.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Data
{
    public class TransactionData : ITransactionData
    {
        List<TransactionModel> transactions;

        public TransactionData()
        {
            transactions = new List<TransactionModel>() 
            { 
                new TransactionModel { ID = 1, MaskCardNumber = "XXXXXXXXXXX-8464", CurrencyCode = "GBP", CVV = 215, ExpiryDay = 15, ExpiryMonth = 10, ShopperGuid = new Guid ("5102C132883E4BB79C95B472F552E9F6"), 
                                       Amount = 15.99m, MerchantCode = "APPLUK", CreatedDateTime = DateTime.UtcNow, PaymentStatus = PaymentStatus.PaymentSuccessful }
            };

        }

        public TransactionModel AddTransaction(TransactionModel transaction)
        {
            if (transactions.Any())
            {
                transaction.ID = transactions.Max(x => x.ID) + 1;
            }
            else
            {
                transaction.ID = 1;
            }

            transactions.Add(transaction);
            return transaction;
        }

        public IEnumerable<TransactionModel> GetAllTransactions(string merchantCode)
        {
            return transactions.Where(x => x.MerchantCode == merchantCode).OrderBy(x => x.CreatedDateTime);
        }

        public TransactionModel GetTransactionById(string merchantCode,int Id)
        {
            return transactions.SingleOrDefault(x => x.ID == Id && x.MerchantCode == merchantCode);
        }

        public IEnumerable<TransactionModel> GetTransactionsByUserGuid(string merchantCode, Guid guid)
        {
            return transactions.Where(x => x.ShopperGuid == guid && x.MerchantCode == merchantCode).ToList();
        }
    }
}
