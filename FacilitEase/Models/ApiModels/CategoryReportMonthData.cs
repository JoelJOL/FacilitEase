namespace FacilitEase.Models.ApiModels
{
    public class CategoryReportMonthData
    {
        public string CategoryName { get; set; }
        public int ResolvedCount { get; set; }
        public int UnresolvedCount { get; set; }
        public int EscalatedCount { get; set; }
    }
}