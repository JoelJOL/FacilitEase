using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilitEase.Models.EntityModels
{

    [Table("TBL_LOCATION")]
    public class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
