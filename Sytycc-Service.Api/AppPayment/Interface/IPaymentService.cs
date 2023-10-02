using Sytycc_Service.Domain;
using Stripe;

namespace Sytycc_Service.Api;

public interface IPaymentService{
      

    Task<PaymentIntent> InitiatePayment(string courseReference,string participantReference, PaymentMethodDto dto);
    Task<PaymentIntent> ConfirmPayment(string paymentIntentId);
}

