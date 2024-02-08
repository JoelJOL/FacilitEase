using System;

namespace FacilitEase.Models.ApiModels
{
    /// <summary>
    /// Api Model to specify the ticket details to be shown when a manager is viewing details of a single ticket
    /// </summary>
    public class ManagerEmployeeTicketDetailed
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string EmployeeName { get; set; }
        public string AssignedTo { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string priorityName { get; set; }
        public string statusName { get; set; }
        public string Notes { get; set; }
        public string LastUpdate { get; set; }
        public string? TicketDescription { get; set; }
        public string? DocumentLink { get; set; }
    }
}