using System;
using Xunit;
using Moq;
using FluentAssertions;
using Arvato.Payment.WebAPI.Controllers;
using Arvato.Payment.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Arvato.Payment.Application.Models.Request;
using Arvato.Payment.WebAPI.Presenters;
using Arvato.Payment.Application.Models.Response;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Arvato.Payment.Test
{
    public class PaymentControllerTest
    {
        private readonly Mock<IPaymentService> serviceStub = new();
        private readonly Mock<ILogger<PaymentController>> loggerStub = new();

        [Theory(DisplayName = "Card owner field does not have other credit card information")]
        [InlineData("first last other", "4554544",455, "1022")]
        public void Validate_CardOwnerHaveOtherInformation_IsNotValid(string cardOwner,string cardNumber,int cvv,string expiryDate)
        {

            //Arrange
            var req = new CreditCardReq() { CardOwner = cardOwner,CreditCardNumbber=cardNumber,CVV=cvv,ExpiryDate=expiryDate };
            //Act
            var results = ValidateModel(req);

            //Assert
            Assert.NotEqual(0, results.Count);
        }

        [Theory(DisplayName = "Credit card is not expired")]
        [InlineData("first last", "5105105105105100", 455, "1020")]
        public void Validate_CardIsExpired_IsNotValid(string cardOwner, string cardNumber, int cvv, string expiryDate)
        {

            //Arrange
            var req = new CreditCardReq() { CardOwner = cardOwner, CreditCardNumbber = cardNumber, CVV = cvv, ExpiryDate = expiryDate };
            //Act
            var results = ValidateModel(req);

            //Assert
            Assert.NotEqual(0, results.Count);
        }

        [Theory(DisplayName = "All fields are provided")]
        [InlineData("first last", "5105105105105100", 455, "1122")]
        public void Validate_AllFieldProvides_IsValid(string cardOwner, string cardNumber, int cvv, string expiryDate)
        {

            //Arrange
            var req = new CreditCardReq() { CardOwner = cardOwner, CreditCardNumbber = cardNumber, CVV = cvv, ExpiryDate = expiryDate };
            //Act
            var results = ValidateModel(req);

            //Assert
            Assert.Equal(0, results.Count);
        }

        [Theory(DisplayName = "Number is valid for specified credit card type")]
        [InlineData("first last", "5200828282828210", 455, "1121")] //master card
        [InlineData("first last", "371449635398431", 4555, "1121")] //American Express
        [InlineData("first last", "4000056655665556", 455, "1121")] //Visa
        public void Validate_CardNumberForMasterCard_Visa_AmericanExpress_IsValid(string cardOwner, string cardNumber, int cvv, string expiryDate)
        {

            //Arrange
            var req = new CreditCardReq() { CardOwner = cardOwner, CreditCardNumbber = cardNumber, CVV = cvv, ExpiryDate = expiryDate };
            //Act
            var results = ValidateModel(req);

            //Assert
            Assert.Equal(0, results.Count);
        }

        [Fact(DisplayName = "Validate must return OutPutContentResult<T> after calling")]
        public async Task Validate_OnCall_MustReturn_OutPutContentResult()
        {

            //Arrange
            var req = new CreditCardReq() { CardOwner = "first last other", CreditCardNumbber = "5105105105105100", CVV = 322, ExpiryDate = "1121" };
            CreditCardTypeRes output = new();
            serviceStub.Setup(service => service.ValidateCreditCard(req)).ReturnsAsync(It.IsAny<CreditCardTypeRes>());
            var controller = new PaymentController(serviceStub.Object, loggerStub.Object);
            //Act
            
            IActionResult results = await controller.ValidateCreditCardAsync(req);

            //Assert

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(results);
            okObjectResult.Value.Should().BeOfType(typeof(OutPutContentResult<>));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
