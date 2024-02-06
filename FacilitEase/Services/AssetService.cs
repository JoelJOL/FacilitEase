using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FacilitEase.Services
{
    public class AssetService : IAssetService
    {
        private readonly AppDbContext _context;

        public AssetService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Asset> GetAssetsByEmployeeId(int employeeId)
        {
            var query = from aem in _context.TBL_ASSET_EMPLOYEE_MAPPING
                        join a in _context.TBL_ASSET on aem.AssetId equals a.Id
                        join at in _context.TBL_ASSET_TYPE on a.TypeId equals at.Id
                        where aem.AssignedTo == employeeId
                        select new Asset
                        {
                            AssetId = a.Id,
                            AssetName = a.AssetName,
                            WarrantyInfo = a.WarrantyInfo,
                            LastMaintenanceDate = a.LastMaintenanceDate,
                            NextMaintenanceDate = a.NextMaintenanceDate,
                            AssetType = at.Type
                        };

            return query.ToList();
        }

        public IEnumerable<Asset> GetUnassignedAssets()
        {
            var unassignedAssets = from a in _context.TBL_ASSET
                                   join at in _context.TBL_ASSET_TYPE on a.TypeId equals at.Id
                                   where a.StatusId == 2
                                   select new Asset
                                   {
                                       AssetId = a.Id,
                                       AssetName = a.AssetName,
                                       WarrantyInfo = a.WarrantyInfo,
                                       LastMaintenanceDate = a.LastMaintenanceDate,
                                       NextMaintenanceDate = a.NextMaintenanceDate,
                                       AssetType = at.Type
                                   };

            return unassignedAssets.ToList();
        }

        public void AssignUnassignedAssets(int employeeId)
        {
            var unassignedAssets = from a in _context.TBL_ASSET
                                   where !_context.TBL_ASSET_EMPLOYEE_MAPPING.Any(e => e.AssetId == a.Id)
                                   select a;

            foreach (var asset in unassignedAssets)
            {
                // Assuming you have a method to create a new assignment record
                AssignAssetToEmployee(asset.Id, employeeId);
            }

            _context.SaveChanges();
        }

        private void AssignAssetToEmployee(int assetId, int employeeId)
        {
            // Create a new assignment record
            var assignment = new TBL_ASSET_EMPLOYEE_MAPPING
            {
                AssetId = assetId,
                AssignedTo = employeeId,
                Status = "Assigned",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _context.TBL_ASSET_EMPLOYEE_MAPPING.Add(assignment);
        }
    }
}
