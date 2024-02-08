namespace FacilitEase.Models.ApiModels
{
    public class AzureReturn
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
        public string LocalAccountId { get; set; }
        public int Expiration { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
    }
}
