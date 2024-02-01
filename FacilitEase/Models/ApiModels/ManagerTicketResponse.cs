namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// to return the retrieved ticket details and its number for pagination and sorting
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ManagerTicketResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalDataCount { get; set; }
    }
}
