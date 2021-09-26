using Arvato.Payment.Application.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.Payment.Application.Models.Request
{
    public class CreditCardReq :IValidatableObject
    {
        [Required]
        [MaxLength(300)]
        [RegularExpression("^(?:[A-Za-z]+ ?){1,2}$",ErrorMessage ="Card owner can't have more than 2 parts First & Last name")]
        public string CardOwner { get; set; }

        [Required]
        [RegularExpression("(0[1-9]|1[0-2])(1[2-9]|[2-9][0-9])",ErrorMessage ="Expire date must be in the format MMYY ")]
        public string ExpiryDate{ get; set; }

        [Required]
        [CreditCard]
        public string CreditCardNumbber { get; set; }

        [Required]
        [RegularExpression(@"^\d{3,4}$",ErrorMessage ="Format of CVV is incorrect")]
        public int CVV { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Check Expiry date and card isn't expired
            var month = int.Parse(ExpiryDate.Substring(0, 2));
            var year = int.Parse("20"+ExpiryDate.Substring(2, 2));
            var lastDayofMonth = DateTime.DaysInMonth(year, month);
            DateTime cardExpiry = new DateTime(year, month, lastDayofMonth,23,59,59);
            if (cardExpiry <= DateTime.Now || cardExpiry > DateTime.Now.AddYears(6))
                yield return new ValidationResult("Card is expired", new[] { nameof(ExpiryDate) });
            //Check cvv number is valid based on Card type
            var cardType = Utility.GetCreditCardTypeByCardNumber(CreditCardNumbber);
            if ((cardType.value == CreditCardType.MasterCard.value || cardType.value == CreditCardType.Visa.value) && CVV.ToString().Length != 3)
                yield return new ValidationResult("CVV is invalid", new[] { nameof(CVV) });
            if (cardType.value == CreditCardType.AmericanExpress.value && CVV.ToString().Length != 4)
                yield return new ValidationResult("CVV is invalid", new[] { nameof(CVV) });
            //Check valid card number
            if (cardType.value == CreditCardType.UnKnown.value)
                yield return new ValidationResult("Card number is invalid", new[] { nameof(CreditCardNumbber) });

        }
    }
}
