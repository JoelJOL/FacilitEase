namespace FacilitEase.Services
{
    public interface IEmailToTicketProcessor
    {
        Task ReadEmailsAndCreateTickets();
    }

}
