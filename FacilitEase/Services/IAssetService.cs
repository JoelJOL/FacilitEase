using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IAssetService
    {
        EmployeeAssetsResponse<Asset> GetAssetsByEmployeeId(int employeeId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        UnassignedAssetResponse<Asset> GetUnassignedAssets(string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
        Asset GetUnassignedAssetDetails(int unassignedAssetId);
        List<AssetHistory> GetDetailsForUnassignedAsset(int unassignedAssetId);
    }
}