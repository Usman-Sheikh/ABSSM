using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABSM.ViewModels;

namespace ABSM.Services
{
    public interface IGateway
    {
        PaymentResult ProcessPayment(CheckoutViewModel model);
    }
}