using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly MailJetService _mailjetService;
        private readonly EmailToTicketProcessor _emailProcessor;

        public EmailController(MailJetService mailjetService, EmailToTicketProcessor emailProcessor)
        {
            _mailjetService = mailjetService;
            _emailProcessor = emailProcessor;
        }

        /* [HttpPost("send")]
         public async Task<IActionResult> SendEmail([FromBody] EmailModel emailModel)
         {
             if (emailModel == null)
             {
                 return BadRequest("Email model is null");
             }

             try
             {
                 await _mailjetService.SendEmailAsync(emailModel.ToEmail, emailModel.Subject, emailModel.Body);
                 return Ok("Email sent successfully!");
             }
             catch (Exception ex)
             {
                 // Log the exception
                 return StatusCode(500, $"Failed to send email. Error: {ex.Message}");
             }
         }*/

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailModel model)
        {
            try
            {
                await _mailjetService.SendEmailAsync(model.ToEmail, model.Subject, model.Body);
                return Ok(new { message = "Email sent successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }
        }

        /*[HttpPost("sendAcknowledgement")]
        public async Task<IActionResult> SendAcknowledgementEmail([FromBody] EmailModel model)
        {
            try
            {
                await _mailjetService.SendAcknowledgementEmail(model.ToEmail);
                return Ok("Acknowledgement email sent successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send acknowledgement email: {ex.Message}");
            }
        }*/

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserEmail>> GetUserEmail(int userId)
        {
            try
            {
                var userEmail = await _mailjetService.GetUserEmailByIdAsync(userId);

                if (userEmail != null)
                    return Ok(userEmail);
                else
                    return NotFound($"User with Id {userId} not found");
            }
            catch
            {
                return StatusCode(500, "Failed to retrieve user email");
            }
        }

        //Email to ticket trigger post
        [HttpPost("process")]
        public async Task<IActionResult> ProcessEmails()
        {
            try
            {
                await _emailProcessor.ReadEmailsAndCreateTickets();
                return Ok("Emails processed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while processing emails: {ex.Message}");
            }
        }

    }
}