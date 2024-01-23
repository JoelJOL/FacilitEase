using System.Collections.Generic;
using System.Threading.Tasks;
using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Controllers
{
    [EnableCors("AllowAngularDev")]
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        // GET: api/Manager
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManagerAPI>>> GetManagers()
        {
            var managers = await _managerService.GetManagersAsync();
            return Ok(managers);
        }
    }
}
