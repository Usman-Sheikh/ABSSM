using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSM.Services
{
    public class PaymentResult
    {
        public PaymentResult(string transactionId, bool success, string message)
        {
            TransactionId = transactionId;
            Succeeded = success;
            Message = message;
        }

        public string TransactionId { get; private set; }
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
    }
}