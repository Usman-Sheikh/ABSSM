using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Braintree;

namespace ABSM.Services
{
    public class PaymentGateway : IGateway
    {
        /*These API keys have been disabled. Always keep API keys private! Never share them with others or commit them to a public GitHub repository.*/
        private readonly BraintreeGateway _gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "p7b7x3mywrkbx3sp",
            PublicKey = "6htsjk8hf3fj5zwr",
            PrivateKey = "fd14b0c82d721414322d4d07a0532c13"
        };

        public PaymentResult ProcessPayment(ViewModels.CheckoutViewModel model)
        {
            var request = new TransactionRequest()
            {
                Amount = model.Total,
                CreditCard = new TransactionCreditCardRequest()
                {
                    Number = model.CardNumber,
                    CVV = model.Cvv,
                    ExpirationMonth = model.Month,
                    ExpirationYear = model.Year
                },
                Options = new TransactionOptionsRequest()
                {
                    SubmitForSettlement = true
                }
            };

            var result = _gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                return new PaymentResult(result.Target.Id, true, null);
            }

            return new PaymentResult(null, false, result.Message);
        }
    }
}
