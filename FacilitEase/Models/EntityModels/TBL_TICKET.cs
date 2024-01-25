using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_TICKET
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string TicketName { get; set; }

        [MaxLength(255)]
        public string TicketDescription { get; set; }

        [Required]
        public DateTime SubmittedDate { get; set; }

        [Required]
        public int PriorityId { get; set; }

        [JsonIgnore]
        public TBL_PRIORITY? Priority { get; set; }

        [Required]
        public int StatusId { get; set; }


        [JsonIgnore]
        public TBL_STATUS? Status { get; set; }

        [Required]
        public int? UserId { get; set; }

        [JsonIgnore]
        public TBL_USER? User { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public TBL_CATEGORY? Category { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [JsonIgnore]
        public TBL_CATEGORY? Department { get; set; }

        public int? AssignedTo { get; set; }

        public int ControllerId { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int? ControllerId { get; set; }

    }
}
