using Checkout.Domain.V1;
using Checkout.Domain.V1.Models;
using System.Collections.Generic;

namespace Checkout.Services
{
    public interface IMerchantService
    {
        bool IsMerchantValid(string code);
        PaymentStatus TakePayment(string merchantCode, PaymentModel shopperPayment);

        IEnumerable<TransactionModel> GetAllTransactions(string merchantCode);

        TransactionModel GetTransactionByID (string merchantCode, int transactionId);
    }
}
