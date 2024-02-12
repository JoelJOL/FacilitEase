using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using System.Linq.Dynamic.Core;

namespace FacilitEase.Services
{
    public class AssetService : IAssetService
    {
        private readonly AppDbContext _context;

        public AssetService(AppDbContext context)
        {
            _context = context;
        }

        public EmployeeAssetsResponse<Asset> GetAssetsByEmployeeId(int employeeId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var employeeAssets = from aem in _context.TBL_ASSET_EMPLOYEE_MAPPING
                                 join asset in _context.TBL_ASSET on aem.AssetId equals asset.Id
                                 join assetType in _context.TBL_ASSET_TYPE on asset.TypeId equals assetType.Id
                                 where aem.AssignedTo == employeeId
                                 where string.IsNullOrEmpty(searchQuery) || asset.AssetName.Contains(searchQuery) || assetType.Type.Contains(searchQuery)
                                 select new Asset
                                 {
                                     Id = asset.Id,
                                     AssetName = asset.AssetName,
                                     WarrantyInfo = asset.WarrantyInfo,
                                     LastMaintenanceDate = asset.LastMaintenanceDate.Value.ToString("yyyy-MM-dd hh:mm tt"),
                                     NextMaintenanceDate = asset.NextMaintenanceDate.Value.ToString("yyyy-MM-dd hh:mm tt"),
                                     AssetType = assetType.Type
                                 };

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                employeeAssets = employeeAssets.OrderBy(orderByString);
            }

            // Apply Pagination
            var totalCount = employeeAssets.Count();
            var paginatedAssets = employeeAssets.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the results in a paginated response object.
            return new EmployeeAssetsResponse<Asset>
            {
                Data = paginatedAssets,
                TotalDataCount = totalCount
            };
        }

        public UnassignedAssetResponse<Asset> GetUnassignedAssets(string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var unassignedAssetsQuery = from asset in _context.TBL_ASSET
                                        join status in _context.TBL_ASSET_STATUS on asset.StatusId equals status.Id
                                        join assetType in _context.TBL_ASSET_TYPE on asset.TypeId equals assetType.Id
                                        where status.Id == 2
                                        where string.IsNullOrEmpty(searchQuery) || asset.AssetName.Contains(searchQuery) || assetType.Type.Contains(searchQuery)
                                        select new Asset
                                        {
                                            Id = asset.Id,
                                            AssetName = asset.AssetName,
                                            WarrantyInfo = asset.WarrantyInfo,
                                            LastMaintenanceDate = asset.LastMaintenanceDate.Value.ToString("yyyy-MM-dd hh:mm tt"),
                                            NextMaintenanceDate = asset.NextMaintenanceDate.Value.ToString("yyyy-MM-dd hh:mm tt"),
                                            AssetType = assetType.Type
                                        };

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                unassignedAssetsQuery = unassignedAssetsQuery.OrderBy(orderByString);
            }

            // Apply Pagination
            var totalCount = unassignedAssetsQuery.Count();
            var paginatedAssets = unassignedAssetsQuery.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the results in a paginated response object.
            return new UnassignedAssetResponse<Asset>
            {
                Data = paginatedAssets,
                TotalDataCount = totalCount
            };
        }

        public Asset GetUnassignedAssetDetails(int unassignedAssetId)
        {
            var query = from asset in _context.TBL_ASSET
                        join status in _context.TBL_ASSET_STATUS on asset.StatusId equals status.Id
                        join assetType in _context.TBL_ASSET_TYPE on asset.TypeId equals assetType.Id
                        where asset.Id == unassignedAssetId
                        select new Asset
                        {
                            Id = asset.Id,
                            AssetName = asset.AssetName,
                            WarrantyInfo = asset.WarrantyInfo,
                            LastMaintenanceDate = asset.LastMaintenanceDate.Value.ToString("yyyy-MM-dd hh:mm tt"),
                            NextMaintenanceDate = asset.NextMaintenanceDate.Value.ToString("yyyy-MM-dd hh:mm tt"),
                            AssetType = assetType.Type,
                            PurchaseDate = asset.PurchaseDate.Value.ToString("yyyy-MM-dd hh:mm tt"),
                        };

            return query.SingleOrDefault();
        }
        public List<AssetHistory> GetDetailsForUnassignedAsset(int unassignedAssetId)
        {
            var query = from mapping in _context.TBL_ASSET_EMPLOYEE_MAPPING
                        join user in _context.TBL_USER on mapping.AssignedTo equals user.Id
                        join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                        where mapping.AssetId == unassignedAssetId && mapping.Status == "Unassigned"
                        select new AssetHistory
                        {
                            Id = mapping.AssetId,
                            AssignedToEmployeeName = employee.FirstName,
                            FromDate = mapping.FromDate,
                            ToDate = mapping.ToDate
                        };

            return query.ToList();
        }


    }
}

