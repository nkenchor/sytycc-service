using Microsoft.AspNetCore.Mvc;
using Stripe;
using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

[Route("api/payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("initiate/{courseReference}/{participantReference}")]
    public async Task<IActionResult> InitiatePayment(string courseReference, string participantReference, [FromBody] PaymentMethodDto paymentMethodDto)
    {
        try
        {
            var paymentIntent = await _paymentService.InitiatePayment(courseReference,participantReference,paymentMethodDto);
            return Ok(new
            {
                id = paymentIntent.Id,
                obj = paymentIntent.Object,
                amount = paymentIntent.Amount,
                amountCapturable = paymentIntent.AmountCapturable,
                amountReceived = paymentIntent.AmountReceived,
                captureMethod = paymentIntent.CaptureMethod,
                clientSecret = paymentIntent.ClientSecret,
                confirmationMethod=paymentIntent.ConfirmationMethod,
                created = paymentIntent.Created,
                currency = paymentIntent.Currency,
                description = paymentIntent.Description,
                livemode = paymentIntent.Livemode,
                paymentMethodsTypes = paymentIntent.PaymentMethodTypes,
                status = paymentIntent.Status,
            });
        }
        catch (StripeException e)
        {
            throw new ServiceStripeException($"Error fetching course by reference: {e.StripeError.Message}");
    
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }

    [HttpPost("confirm/{paymentIntentId}")]
    public async Task<IActionResult>  ConfirmPayment(string paymentIntentId)
    {
        try
        {
            var paymentIntent = await _paymentService.ConfirmPayment(paymentIntentId);
            return Ok(generatePaymentResponse(paymentIntent));
        }
        catch (AppException e)
        {
            return StatusCode(e.StatusCode, new AppExceptionResponse(e));
        }
    }
     private IActionResult generatePaymentResponse(PaymentIntent intent)
    {
      if (intent.Status == "succeeded")
      {
        // Handle post-payment fulfillment
        return Ok(new { success = true });
      }
      else if (intent.Status == "requires_action")
      {
          // Tell the client to handle the action
          return Ok(new
          {
              requiresAction = true,
              clientSecret = intent.ClientSecret
          });
      }
      else
      {
        // Any other status would be unexpected, so error
        return StatusCode(500, new { error = "Invalid PaymentIntent status" });
      }
    }
}
