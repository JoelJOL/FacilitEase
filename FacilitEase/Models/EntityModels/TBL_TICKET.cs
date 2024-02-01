using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_TICKET
    {
        [Key]
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string? TicketDescription { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int? AssignedTo { get; set; }
        public int? ControllerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

}
