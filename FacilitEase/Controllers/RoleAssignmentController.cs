using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleAssignmentController : ControllerBase
    {
        private readonly IL1AdminService _l1adminService;

        public RoleAssignmentController(IL1AdminService l1adminService)
        {
            _l1adminService = l1adminService;
        }
        /// <summary>
        /// To display the suggestions with respect to the text searched in the searchbar. 
        /// It retrieves the name position and username of employees that have matcheing names to the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet("{text}")]
        public ActionResult<IEnumerable<ProfileData>> GetSuggestions(string text)
        {
            return Ok(_l1adminService.GetSuggestions(text));
            ;
        }

        /// <summary>
        /// To display all the roles available for a user This will only include the roles that are not 
        /// </summary>
        /// <returns></returns>
        [HttpGet("possibleroles/{id}")]
        public ActionResult GetRoles(int id)
        {
            return Ok(_l1adminService.GetRoles(id));
        }

        /// <summary>
        /// Asssigning a role by updating user_role_mapping table
        /// </summary>
        /// <param name="assignrole"></param>
        /// <returns></returns>
        [HttpPost("AssignRoles")]
        public IActionResult AssignRole([FromBody] AssignRole assignrole)
        {
            _l1adminService.AssignRole(assignrole);
            return Ok();
        }
    }
}