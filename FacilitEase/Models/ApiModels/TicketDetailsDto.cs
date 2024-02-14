﻿namespace FacilitEase.Models.ApiModels
{
    public class TicketDetailsDto
    {
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public string Priority { get; set; }
        public string SubmittedDate { get; set; }

        public int UserId { get; set; }
    }
}