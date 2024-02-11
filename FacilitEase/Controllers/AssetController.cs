using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
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

        /*[HttpPost("assign-unassigned-assets/{employeeId}")]
        public IActionResult AssignUnassignedAssets(int employeeId)
        {
            try
            {
                _assetService.AssignUnassignedAssets(employeeId);
                return Ok("Assets assigned successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error assigning assets: {ex.Message}");
            }
        }*/

        [HttpGet("assets/unassigned-assets")]
        public IActionResult GetUnassignedAssets(string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var unassignedAssets = _assetService.GetUnassignedAssets(sortField, sortOrder, pageIndex, pageSize, searchQuery);
            return Ok(unassignedAssets);
        }
    }
}