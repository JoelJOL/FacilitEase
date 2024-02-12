namespace FacilitEase.Models.EntityModels
{
    public class TBL_ASSET_EMPLOYEE_MAPPING
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int AssignedTo { get; set; }
        public string Status { get; set; }
        public int? TicketId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}