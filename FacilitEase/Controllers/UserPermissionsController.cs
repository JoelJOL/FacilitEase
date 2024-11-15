using FacilitEase.Contracts.ServiceContracts;
using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPermissionsController : ControllerBase
    {
        private readonly IUserPermissionsService _userPermissionsService;

        public UserPermissionsController(IUserPermissionsService userPermissionsService)
        {
            _userPermissionsService = userPermissionsService;
        }

        /// <summary>
        /// To display the suggestions with respect to the text searched in the searchbar.
        /// It retrieves the name position and username of employees that have matcheing names to the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<UserPermissionData>> GetUserPermissions(int userId)
        {
            var permissions = _userPermissionsService.GetUserPermissions(userId);
            return Ok(permissions);
        }

        [HttpPatch("{userPermissionId}")]
        public IActionResult UpdateUserPermission(int userPermissionId, PermissionDetails permissionDetails)
        {
            bool success = _userPermissionsService.UpdatePermission(userPermissionId, permissionDetails.IsActive);

            if (success)
            {
                return Ok(new { Message = "Ticket cancellation request successful." });
            }
            else
            {
                return NotFound(new { Message = "Ticket not found or cancellation request failed." });
            }
        }
    }
}