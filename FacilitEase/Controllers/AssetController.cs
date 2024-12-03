using FacilitEase.Contracts.ServiceContracts;
using FacilitEase.Models.ApiModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [Route("api")]
    [EnableCors("AllowAngularDev")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet("assets/{employeeId}")]
        public IActionResult GetAssetsByEmployeeId(int employeeId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var assets = _assetService.GetAssetsByEmployeeId(employeeId, sortField, sortOrder, pageIndex, pageSize, searchQuery);
            return Ok(assets);
        }

        [HttpGet("unassigned-assets")]
        public IActionResult GetUnassignedAssets(string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var unassignedAssets = _assetService.GetUnassignedAssets(sortField, sortOrder, pageIndex, pageSize, searchQuery);
            return Ok(unassignedAssets);
        }

        [HttpGet("unassigned-asset/{assetId}")]
        public ActionResult<Asset> GetUnassignedAssetDetails(int assetId)
        {
            var assetDetails = _assetService.GetUnassignedAssetDetails(assetId);

            if (assetDetails == null)
            {
                return NotFound();
            }

            return Ok(assetDetails);
        }

        [HttpGet("asset-history/{assetId}")]
        public ActionResult<AssetHistory> GetDetailsForUnassignedAsset(int assetId)
        {
            var assetHistory = _assetService.GetDetailsForUnassignedAsset(assetId);

            if (assetHistory == null)
            {
                return NotFound();
            }

            return Ok(assetHistory);
        }
    }
}