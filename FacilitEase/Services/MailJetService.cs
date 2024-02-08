using Mailjet.Client;
using System.Net.Mail;
using Mailjet.Client.Resources;
using System;
using System.Threading.Tasks;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using FacilitEase.Data;
using FacilitEase.Repositories;
using FacilitEase.UnitOfWork;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public class MailJetService
    {
        private readonly string apiKey = DotNetEnv.Env.GetString("MailJetApiKey");
        private readonly string apiSecret = DotNetEnv.Env.GetString("MailApiSecretKey");
        private readonly AppDbContext _context;//Abhijith
        
        public MailJetService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserEmail> GetUserEmailByIdAsync(int userId)
        {
            Console.WriteLine(apiKey);
            var user = await _context.TBL_USER
                .Where(u => u.Id == userId)
                .Join(
                    _context.TBL_EMPLOYEE,
                    user => user.EmployeeId,
                    employee => employee.Id,
                    (user, employee) => new
                    {
                        user.Id,
                        user.Email,
                        employee.FirstName,
                        employee.LastName
                    }
                )
                .FirstOrDefaultAsync();

            if (user != null)
            {
                var fullName = $"{user.FirstName} {user.LastName}";
                return new UserEmail { Email = user.Email, UserName = fullName };
            }

            return null; // or handle the case where the user is not found
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient("in-v3.mailjet.com"))
            {
                client.UseDefaultCredentials = false;
                Console.WriteLine(apiKey);
                Console.WriteLine(apiSecret);
                client.Credentials = new NetworkCredential(apiKey, apiSecret);
              

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("nathanielyeldo22@gmail.com", "Nathaniel Yeldo"),
                    Subject = subject,
                    Body = body,
                };

                mailMessage.To.Add(new MailAddress("nathanielyeldo22@gmail.com", "Nathaniel Yeldo"));

                try
                {
                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine("Email sent successfully!");
                    await SendAcknowledgementEmail(toEmail);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }

        private async Task SendAcknowledgementEmail(string toEmail)
        {
            const string acknowledgementSubject = "Support Acknowledgement";
            const string acknowledgementBody = "Thank you for your query. We have received your message and will get back to you soon.";
            using (var client = new SmtpClient("in-v3.mailjet.com"))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(apiKey, apiSecret);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("nathanielyeldo22@gmail.com", "Nathaniel Yeldo"),
                    Subject = acknowledgementSubject,
                    Body = acknowledgementBody,
                };

                mailMessage.To.Add(new MailAddress(toEmail));

                try
                {
                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine("Acknowledgement Email sent successfully!");


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
    }
}
