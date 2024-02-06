using FacilitEase.Models.ApiModels;
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

        [HttpGet("employee/{employeeId}")]
        public IActionResult GetAssetsByEmployeeId(int employeeId)
        {
            var assets = _assetService.GetAssetsByEmployeeId(employeeId);
            return Ok(assets);
        }
        [HttpPost("assign-unassigned-assets/{employeeId}")]
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
        }

        [HttpGet("asset/GetUnassignedAssets")]
        public ActionResult<IEnumerable<Asset>> GetUnassignedAssets()
        {
            var unassignedAssets = _assetService.GetUnassignedAssets();
            return Ok(unassignedAssets);
        }
    }
}
