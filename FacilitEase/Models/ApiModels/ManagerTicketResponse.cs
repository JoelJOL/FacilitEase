namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// Api Model for the response when listing tickets for manager
    /// returns all the tickets and total number of employee tickets for pagination and sorting
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ManagerTicketResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalDataCount { get; set; }
    }
}