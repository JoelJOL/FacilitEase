﻿using FacilitEase.Data;
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
                                     LastMaintenanceDate = asset.LastMaintenanceDate,
                                     NextMaintenanceDate = asset.NextMaintenanceDate,
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
                                            LastMaintenanceDate = asset.LastMaintenanceDate,
                                            NextMaintenanceDate = asset.NextMaintenanceDate,
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
    }
}