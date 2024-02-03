using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IL1AdminService
    {
        IEnumerable<ProfileData> GetSuggestions(string text);

        IEnumerable<string> GetRoles(int id);

        void AssignRole(AssignRole assignrole);
    }
}