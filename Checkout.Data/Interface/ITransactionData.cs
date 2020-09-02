using Checkout.Domain.V1.Models;
using System;
using System.Collections.Generic;

namespace Checkout.Data
{
    public interface ITransactionData
    {
        IEnumerable<TransactionModel> GetAllTransactions(string merchantCode);
        IEnumerable<TransactionModel> GetTransactionsByUserGuid(string merchantCode, Guid guid);
        TransactionModel GetTransactionById(string merchantCode, int Id);
        TransactionModel AddTransaction(TransactionModel transaction);
    }
}
