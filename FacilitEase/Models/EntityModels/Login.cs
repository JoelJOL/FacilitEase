using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace FacilitEase.Models.EntityModels
{
    [Table("TBL_LOGIN")]
    public class TBL_LOGIN
    {
        public int Id {  get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
    }
}
