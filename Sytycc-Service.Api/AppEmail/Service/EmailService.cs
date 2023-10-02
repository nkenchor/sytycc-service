using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Serilog;
using System.Globalization;
using Sytycc_Service.Domain;

namespace Sytycc_Service.Api;

public class EmailService : IEmailService
{
   
    private readonly IRegistrationService _registrationService;

    public EmailService(IRegistrationService registrationService)
    {
       
       _registrationService = registrationService;
    }

    public async Task<EmailResult> SendEmailNotificationToParticipant(RegistrationReferenceDto registrationReferenceDto)
    {
        var emailContent = await GetEmailBody(registrationReferenceDto.RegistrationReference, 
                                            @"../template/customertemplate.html", 
                                            EmailType.Participant);
        if (string.IsNullOrEmpty(emailContent.emailContent))
        {
            Log.Error("Email content is null or empty for Participant.");
            return new EmailResult { IsSuccess = false, Message = "Failed to generate email content." };
        }

        return SendEmail(emailContent.recipientEmail, "Payment Confirmation", emailContent.emailContent);
    }

    public async Task<EmailResult> SendEmailNotificationToAdmin(RegistrationReferenceDto registrationReferenceDto)
    {
        var emailContent = await GetEmailBody(registrationReferenceDto.RegistrationReference, 
                                            @"../template/admintemplate.html", 
                                            EmailType.Admin);
        if (string.IsNullOrEmpty(emailContent.emailContent))
        {
            Log.Error("Email content is null or empty for Admin.");
            return new EmailResult { IsSuccess = false, Message = "Failed to generate email content." };
        }

        return SendEmail(emailContent.recipientEmail, "Payment Details", emailContent.emailContent);
    }





    private EmailResult SendEmail(string recipient, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(Service.MailFrom));
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;
            var builder = new BodyBuilder
            {
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(Service.MailHost, int.Parse(Service.MailPort), SecureSocketOptions.SslOnConnect);
            smtp.Authenticate(Service.UserName, Service.Password);
            smtp.Send(email);
            smtp.Disconnect(true);

            Log.Information($"Email sent successfully to {recipient} with subject: {subject}");

            return new EmailResult { IsSuccess = true, Message = "Email sent successfully" };
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to send email to {recipient} with subject: {subject}. Error: {ex.Message}");

            return new EmailResult { IsSuccess = false, Message = $"Failed to send email. Error: {ex.Message}" };
        }
    }


    private async Task<(string recipientEmail, string? emailContent)> GetEmailBody(string registrationReference, string emailTempPath, EmailType emailType)
    {
        try
        {
            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            var registrationDto = await _registrationService.GetRegistrationByReference(registrationReference);
            
            if (registrationDto == null)
            {
                Log.Error("No registration data found for the given reference.");
                return (string.Empty, null);
            }

            var participant = registrationDto.Participant ?? throw new Exception("No participant data found.");
            var registration = registrationDto.Registration ?? throw new Exception("No registration details found.");
            var course = registrationDto.Course ?? throw new Exception("No course data found.");
            
            // Common fields
            var customerName = textInfo.ToTitleCase(participant.FullName ?? string.Empty);
            var paymentAmount = textInfo.ToTitleCase(registration.AmountPaid.ToString());
            var courseTitle = textInfo.ToTitleCase(course.Title ?? string.Empty);
            var startDate = FormatDate(course.StartDate);
            var endDate = FormatDate(course.EndDate);
            var startTime = course.StartTime; 
            var endTime = course.EndTime;     

            
            // Read the template
            Log.Information("Getting the template...");
            var temp = await File.ReadAllTextAsync(emailTempPath);
            Log.Information($"Successful get email path: {temp}");

            if(emailType == EmailType.Participant)
            {
                var emailContent = temp.Replace("**CustomerName**", customerName)
                .Replace("**PaymentAmount**", paymentAmount)
                .Replace("**CourseTitle**", courseTitle)
                .Replace("**StartDate**", startDate)
                .Replace("**EndDate**", endDate)
                .Replace("**StartTime**", startTime)
                .Replace("**EndTime**", endTime);
                return (registrationDto.Participant.Email, emailContent);
            }
            else if(emailType == EmailType.Admin)
            {
                var emailContent = temp.Replace("**ReceiptEmail**", textInfo.ToTitleCase(Service.MailFrom))
                        .Replace("**Amount**", paymentAmount)
                        .Replace("**PaymentTime**", textInfo.ToTitleCase(registrationDto.Registration.RegistrationTime.ToString()))
                        .Replace("**Name**", customerName)
                        .Replace("**Email**", textInfo.ToTitleCase(registrationDto.Participant.Email))
                        .Replace("**Title**", courseTitle)
                        .Replace("**Level**", textInfo.ToTitleCase(registrationDto.Course.Level.ToString()))
                        .Replace("**TrainingType**", textInfo.ToTitleCase(registrationDto.Course.TrainingType.ToString()));
                return (Service.MailFrom, emailContent);
            }
        }
        catch (AppException e)
        {
            Log.Error($"Database Error: {e.Message}");
            throw;
        }
        // General error (this should be last in the catch sequence)
        catch (Exception e)
        {
            Log.Error($"Error Creating Facilitator: {e.Message}");
            throw new InternalServerException(e.Message);
        }

        return (string.Empty, null); // This will serve as a fallback in case neither condition is met.
    }

    private string FormatDate(string dateStr)
    {
        DateTime.TryParseExact(dateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
        return date.ToString("dd'th' of MMMM");
    }
  

}

