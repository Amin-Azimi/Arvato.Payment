using Arvato.Payment.Application.Models;
using Arvato.Payment.Application.Models.Request;
using Arvato.Payment.Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.Payment.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreditCardTypeRes> ValidateCreditCard(CreditCardReq req);
    }
}
