﻿namespace FacilitEase.Models.ApiModels
{
    public class TicketDto
    {
        /*public int Id { get; set; }
        public string TicketName { get; set; }
        public string? TicketDescription { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int? AssignedTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }*/
        
            public string TicketName { get; set; }
            public string TicketDescription { get; set; }
            public int PriorityId { get; set; }
            public int CategoryId { get; set; }
            public int DepartmentId { get; set; }
            public string[]? DocumentLink { get; set; }

    }
}