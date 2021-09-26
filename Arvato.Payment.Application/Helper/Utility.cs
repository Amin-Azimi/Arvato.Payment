using Arvato.Payment.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arvato.Payment.Application.Helper
{
    public static class Utility
    {
        public static CreditCardType GetCreditCardTypeByCardNumber(string cardNumber)
        {
            CreditCardType cardType = CreditCardType.UnKnown;
            if (Regex.IsMatch(cardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$"))
                cardType = CreditCardType.Visa;
            else if (Regex.IsMatch(cardNumber, @"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$"))
                cardType = CreditCardType.MasterCard;
            else if (Regex.IsMatch(cardNumber, @"^3[47][0-9]{13}$"))
                cardType = CreditCardType.AmericanExpress;
            return cardType;
        }
    }
}
