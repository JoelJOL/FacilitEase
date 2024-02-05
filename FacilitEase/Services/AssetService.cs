using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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
                var assets = (from mapping in _context.TBL_ASSET_EMPLOYEE_MAPPING
                              join asset in _context.TBL_ASSET on mapping.AssetId equals asset.Id
                              join location in _context.TBL_LOCATION on asset.LocationId equals location.Id
                              join assetType in _context.TBL_ASSET_TYPE on asset.TypeId equals assetType.Id
                              join assetStatus in _context.TBL_ASSET_STATUS on asset.StatusId equals assetStatus.Id
                              join employee in _context.TBL_EMPLOYEE on mapping.EmployeeId equals employee.Id
                              join ticket in _context.TBL_TICKET on mapping.TicketId equals ticket.Id into ticketJoin
                              from ticket in ticketJoin.DefaultIfEmpty()
                              where mapping.EmployeeId == employeeId
                              select new Asset
                              {
                                  AssetId = asset.Id,
                                  AssetName = asset.AssetName,
                                  WarrantyInfo = asset.WarrantyInfo,
                                  LastMaintenanceDate = asset.LastMaintenanceDate,
                                  NextMaintenanceDate = asset.NextMaintenanceDate,
                                  LocationName = location.LocationName,
                                  TypeName = assetType.Type,
                                  StatusName = assetStatus.Status,
                                  EmployeeName = employee.FirstName + " " + employee.LastName,
                                  Status = mapping.Status,
                                  TicketId = mapping.TicketId,
                                  CreatedDate = mapping.CreatedDate,
                                  UpdatedDate = mapping.UpdatedDate
                              })
                              .ToList();

                return assets;
            }

        
    }
}