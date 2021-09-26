using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arvato.Payment.WebAPI.Presenters
{
    public class ErrorContent
    {
        public string ErrorField { get; set; }
        public string ErrorDescription { get; set; }
    }
}
