﻿namespace FacilitEase.Models.EntityModels
{
    public class TBL_COMMENT
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}