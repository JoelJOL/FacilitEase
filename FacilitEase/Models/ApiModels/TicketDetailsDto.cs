﻿namespace FacilitEase.Models.ApiModels
{
    public class TicketDetailsDto
    {
        public string TicketName { get; set; }
        public string TicketDescription { get; set; }
        public string StatusId { get; set;}
        public string AssignedTo { get; set; }
        public string PriorityId { get; set; }
    }
}
