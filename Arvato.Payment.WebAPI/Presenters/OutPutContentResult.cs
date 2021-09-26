using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arvato.Payment.WebAPI.Presenters
{
    public class OutPutContentResult<T> 
    {
        public IEnumerable<T> data { get; set; }
        public IEnumerable<ErrorContent> errors { get; set; }
    }
}
