using Checkout.Data;
using Checkout.Domain.V1;
using Checkout.Domain.V1.Models;
using System;
using System.Collections.Generic;

namespace Checkout.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IBankService _bankService;
        private readonly IMerchantData _merchantData;
        private readonly ITransactionData _transactionData;

        public MerchantService(IBankService bankService, ITransactionData transactionData, IMerchantData merchantData)
        {
            _bankService = bankService;
            _transactionData = transactionData;
            _merchantData = merchantData;
        }

        public PaymentStatus TakePayment(string merchantCode, PaymentModel shopperPayment)
        {
            PaymentStatus status = PaymentStatus.Pending;
            var shopperGuid = new Guid(shopperPayment.ShopperGuid);
            var cardNumberExceptLastFour = shopperPayment.CardNumber.ToString().Substring(0, shopperPayment.CardNumber.ToString().Length - 4);            
            var shopperMaskedCardNo = shopperPayment.CardNumber.ToString().Replace(cardNumberExceptLastFour, "XXXXXXXXXXX-");

            var transactionLog = new TransactionModel()
            {
                ID = 0,
                MaskCardNumber = shopperMaskedCardNo,
                Amount = shopperPayment.Amount,
                CurrencyCode = shopperPayment.CurrencyCode,
                ExpiryDay = shopperPayment.ExpiryDay,
                ExpiryMonth = shopperPayment.ExpiryMonth,
                MerchantCode = merchantCode,
                CVV = shopperPayment.CVV,
                ShopperGuid = shopperGuid,
                PaymentStatus = PaymentStatus.Pending,
                CreatedDateTime = DateTime.UtcNow
            };

            try
            {
                if (!_bankService.ValidateCardDetails(shopperPayment))
                {
                    status = PaymentStatus.FailedCardVerification;
                    transactionLog.PaymentStatus = status;
                    _transactionData.AddTransaction(transactionLog);
                    return status;
                }


                if(!_bankService.CanShopperPay(shopperPayment.Amount, shopperGuid))
                {
                    status = PaymentStatus.InsufficientFunds;
                    transactionLog.PaymentStatus = status;
                    _transactionData.AddTransaction(transactionLog);
                    return status;
                }

                _merchantData.ProcessMerchantPayment(merchantCode, shopperPayment.Amount);
                var accountUpdated = _bankService.UpdateShopperAccount(shopperGuid, shopperPayment.Amount);    
                status = accountUpdated ? PaymentStatus.PaymentSuccessful : PaymentStatus.Failed;
            }
            catch (Exception)
            {
                //Log Error
                status = PaymentStatus.Failed;
            }

            //Log Transaction Details
            transactionLog.PaymentStatus = status;
            _transactionData.AddTransaction(transactionLog);

            return status;
        }

        public IEnumerable<TransactionModel> GetAllTransactions(string merchantCode)
        {
            return _transactionData.GetAllTransactions(merchantCode);
        }

        public TransactionModel GetTransactionByID(string merchantCode, int transactionId)
        {
            return _transactionData.GetTransactionById(merchantCode, transactionId);
        }

        public bool IsMerchantValid(string code)
        {
            return _merchantData.isMerchantValid(code);
        }
    }
}
