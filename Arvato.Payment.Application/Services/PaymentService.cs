using Arvato.Payment.Application.Helper;
using Arvato.Payment.Application.Models;
using Arvato.Payment.Application.Models.Request;
using Arvato.Payment.Application.Models.Response;
using Arvato.Payment.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.Payment.Application.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<CreditCardTypeRes> ValidateCreditCard(CreditCardReq req)
        {

            CreditCardTypeRes _creditCardTypeRes = new();
            _creditCardTypeRes.CardName = Utility.GetCreditCardTypeByCardNumber(req.CreditCardNumbber).value;
            return _creditCardTypeRes;
        }
    }
}
