namespace FacilitEase.Models.ApiModels
{
    public class EmployeeTicketResponse<T>
    {
      
            public IEnumerable<T> Data { get; set; }
            public int TotalDataCount { get; set; }
    }
}
