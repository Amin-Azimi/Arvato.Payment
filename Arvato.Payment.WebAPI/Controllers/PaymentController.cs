using Arvato.Payment.Application.Models.Request;
using Arvato.Payment.Application.Models.Response;
using Arvato.Payment.Application.Services.Interfaces;
using Arvato.Payment.WebAPI.Presenters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arvato.Payment.WebAPI.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService paymentService;
        private readonly ILogger<PaymentController> logger;
        public PaymentController(IPaymentService _paymentService, ILogger<PaymentController> _logger)
        {
            paymentService = _paymentService;
            logger = _logger;
        }

        [HttpPost("validate")]
        public async Task<ActionResult> ValidateCreditCardAsync(CreditCardReq req)
        {
            OutPutContentResult<CreditCardTypeRes> output = new();
            CreditCardTypeRes creditCardType = new();
            try
            {
                creditCardType = await paymentService.ValidateCreditCard(req);
                IEnumerable<CreditCardTypeRes> list = new List<CreditCardTypeRes>() { creditCardType };
                output.data = list;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500);
            }
            return Ok(output);
        }

    }
}
