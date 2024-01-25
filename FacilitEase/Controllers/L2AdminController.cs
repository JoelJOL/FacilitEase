using FacilitEase.Models.ApiModels;
using FacilitEase.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FacilitEase.Controllers
{
    [ApiController]
    [Route("api/l2")]
    public class L2AdminController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ITicketService _ticketService;


        public L2AdminController(IEmployeeService employeeService, ITicketService ticketService)
        {
            _employeeService = employeeService;
            _ticketService = ticketService;
        }


        [HttpGet("agents")]
        public IActionResult GetAgents([FromQuery] DepartmentAgentsRequestModel requestModel)
        {
            var agents = _employeeService.GetAgents(requestModel.DepartmentId);
            return Ok(agents);
        }
        [HttpGet("tickets")]
        public ActionResult<List<TicketApiModel>> GetTickets()
        {
            try
            {
                var tickets = _ticketService.GetTickets();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("unassigned-tickets")]
        public ActionResult<List<TicketApiModel>> GetUnassignedTickets()
        {
            try
            {
                var unassignedTickets = _ticketService.GetUnassignedTickets();
                return Ok(unassignedTickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("assign-ticket")]
        public ActionResult AssignTicketToAgent(AssignTicket request)
        {
            try
            {
                _ticketService.AssignTicketToAgent(request.TicketId, request.AgentId);
                return Ok(new { Message = "Ticket assigned successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("assigned-tickets")]
        public ActionResult<List<TicketApiModel>> GetAssignedTickets()
        {
            try
            {
                var assignedTickets = _ticketService.GetAssignedTickets();
                return Ok(assignedTickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("escalated-tickets")]
        public ActionResult<List<TicketApiModel>> GetEscalatedTickets()
        {
            var escalatedTickets = _ticketService.GetEscalatedTickets();
            return Ok(escalatedTickets);
        }
        [HttpGet("agentsByDepartmentId")]
        public IActionResult GetAgentsByDepartment([FromQuery] DepartmentAgentsRequestModel requestModel)
        {
            var agents = _employeeService.GetAgentsByDepartment(requestModel.DepartmentId);
            return Ok(agents);
        }
    }
}
