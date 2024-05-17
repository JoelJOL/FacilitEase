namespace FacilitEase.Contracts.ServiceContracts
{
    public interface IEmailToTicketProcessor
    {
        Task ReadEmailsAndCreateTickets();
    }

}
