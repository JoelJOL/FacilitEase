using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

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

        [HttpGet("{text}")]
       public ActionResult<IEnumerable<ProfileData>> GetSuggestions(string text)
        {
           
            return Ok(_l1adminService.GetSuggestions(text));
;        }
        /// <summary>
        /// To display all the roles available for a user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRoles()
        {
            return Ok(_l1adminService.GetRoles());
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
