namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// Represents a API for updating comment information.
    /// </summary>
    public class UpdateCommentDto
    {
        public int Id { get; set; }
        public string NewText { get; set; }
    }
}