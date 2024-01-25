﻿namespace FacilitEase.Models.ApiModels
{
    public class ManagerEmployeeTicketDetailed
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string EmployeeName { get; set; }
        public string AssignedTo { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        public string? TicketDescription { get; set; }
        public string? DocumentLink { get; set; }

    }
}
