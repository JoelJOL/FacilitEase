// TicketStatusChangeRequest.cs
namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// The api model to change the status of the ticket when deparment head click approve or deny
    /// </summary>
    public class TicketStatusChangeRequest
    {
        public bool IsApproved { get; set; }

        public int? ControllerId { get; set; }

    }
}
