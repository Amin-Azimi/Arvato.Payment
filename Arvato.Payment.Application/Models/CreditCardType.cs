using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.Payment.Application.Models
{
    public class CreditCardType
    {
        public CreditCardType(string _value) => value = _value;
        public string value { get; private set; }

        public static CreditCardType MasterCard { get { return new CreditCardType("Master Card"); } }
        public static CreditCardType Visa { get { return new CreditCardType("Visa"); } }
        public static CreditCardType AmericanExpress { get { return new CreditCardType("American Express"); } }
        public static CreditCardType UnKnown { get { return new CreditCardType("UnKnown"); } }
    }
}
