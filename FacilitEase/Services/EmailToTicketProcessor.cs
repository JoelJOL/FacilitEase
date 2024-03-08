using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;
using FacilitEase.Services;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

public class EmailToTicketProcessor
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TicketService _ticketService;
    private readonly string apiKey = DotNetEnv.Env.GetString("MailJetApiKey");
    private readonly string apiSecret = DotNetEnv.Env.GetString("MailApiSecretKey");

    public EmailToTicketProcessor(IUnitOfWork unitOfWork, TicketService ticketService)
    {
        _unitOfWork = unitOfWork;
        _ticketService = ticketService;
    }

    public async Task ReadEmailsAndCreateTickets()
    {
        using (var client = new ImapClient())
        {
            await client.ConnectAsync("in-v3.mailjet.com", 993, true);
            await client.AuthenticateAsync(apiKey, apiSecret);

            var inbox = client.Inbox;
            await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

            var uids = await inbox.SearchAsync(MailKit.Search.SearchQuery.All);

            foreach (var uid in uids)
            {
                var message = await inbox.GetMessageAsync(uid);

                // Extract the necessary information from the email
                var emailSubject = message.Subject;
                var emailBody = message.TextBody;

                // Get the user ID from the TBL_USER table by checking the email ID of the sender
                var user = _unitOfWork.User.GetAll().FirstOrDefault(u => u.Email == message.From.Mailboxes.First().Address);

                if (user != null)
                {
                    // Create a new ticket using the extracted information
                    var ticketDto = new TicketDto
                    {
                        TicketName = emailSubject,
                        TicketDescription = emailBody,
                        PriorityId = 2, // Assuming 2 is the ID for medium priority
                        CategoryId = 1, // Set this to the appropriate category ID
                        DepartmentId = 1, // Set this to the appropriate department ID
                        UserId = user.Id,
                        CreatedBy = user.Id,
                        UpdatedBy = user.Id,
                        StatusId = 1, // Set this to the appropriate status ID
                        AssignedTo = null, // No one is assigned initially
                        SubmissionDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };

                    // Call the function to create a new ticket
                    if (message.Attachments.Any())
                    {
                        foreach (var attachment in message.Attachments)
                        {
                            using (var stream = new MemoryStream())
                            {
                                if (attachment is MessagePart)
                                {
                                    var part = (MessagePart)attachment;
                                    part.Message.WriteTo(stream);
                                }
                                else
                                {
                                    var part = (MimePart)attachment;
                                    part.Content.DecodeTo(stream);
                                }

                                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(attachment.ContentDisposition?.FileName))
                                {
                                    Headers = new HeaderDictionary(),
                                    ContentType = attachment.ContentType.MimeType
                                };

                                _ticketService.CreateTicketWithDocuments(ticketDto, file);
                            }
                        }
                    }
                    else
                    {
                        _ticketService.CreateTicketWithDocuments(ticketDto, null);
                    }
                }
            }

            await client.DisconnectAsync(true);
        }
    }
}
