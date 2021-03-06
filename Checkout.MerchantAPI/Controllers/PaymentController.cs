﻿using Checkout.Domain.V1;
using Checkout.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Checkout.MerchantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IMerchantService _merchantService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IMerchantService merchantService, ILogger<PaymentController> logger)
        {
            _merchantService = merchantService;
            _logger = logger;
        }


        // GET: api/<PaymentController>/transactions
        /// <summary>
        /// Get payment transaction by Id. Filters by merchant's unique code
        /// </summary>
        /// <param name="id">Id for each payment record</param>
        /// <returns></returns>
        [HttpGet("transactions/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetTransaction(int id)
        {
            var merchantCode = Request.Headers["merchantCode"].ToString();
            if (string.IsNullOrEmpty(merchantCode) || !_merchantService.IsMerchantValid(merchantCode) || id <= 0 )
                return BadRequest("Check merchantCode is valid and transactionId is valid !!");

            try
            {
                var transaction = _merchantService.GetTransactionByID(merchantCode, id);
                if (transaction == null)
                {
                    return NotFound($"No transaction found for ID : {id}");
                }

                return Ok(transaction);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }

        // POST api/<PaymentController>/process
        /// <summary>
        /// Process payment for a particular merchant
        /// </summary>
        /// <param name="paymentModel">Model contains what is required to make payment such as card number, expiry month/date, amount, currency, cvv and shopper's unique identifier</param>
        /// <returns></returns>
        [HttpPost("process")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ProcessPayment([FromBody] PaymentModel paymentModel)
        {
            var merchantCode = Request.Headers["merchantCode"].ToString();
            if (string.IsNullOrEmpty(merchantCode) || !_merchantService.IsMerchantValid(merchantCode) || paymentModel == null)
                return BadRequest("Check merchantCode is valid and paymentModel is not null !!");

            try
            {
                var paymentResult = _merchantService.TakePayment(merchantCode, paymentModel);
                if (paymentResult == PaymentStatus.PaymentSuccessful)
                {
                    return Ok(paymentResult.ToString());
                }
                else
                {
                    return BadRequest(paymentResult.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
