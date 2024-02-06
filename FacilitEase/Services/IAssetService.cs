﻿using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IAssetService
    {
        IEnumerable<Asset> GetAssetsByEmployeeId(int employeeId);
        IEnumerable<Asset> GetUnassignedAssets();
        void AssignUnassignedAssets(int employeeId);
    }

}