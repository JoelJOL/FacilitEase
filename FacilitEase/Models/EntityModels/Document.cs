﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{
    [Table("TBL_DOCUMENT")]
    public class TBL_DOCUMENT
    {
        [Key]
        public int Id { get; set; }
        public string DocumentLink { get; set; }
        public int TicketId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
