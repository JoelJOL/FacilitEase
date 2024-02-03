namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// To get the priority
    /// </summary>
    public class PriorityDto
    {
        public int Id { get; set; }
        public string PriorityName { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
