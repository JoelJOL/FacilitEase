﻿using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_CATEGORY
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int DepartmentId { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
