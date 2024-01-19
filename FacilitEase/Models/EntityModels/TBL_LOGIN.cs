﻿using System.Numerics;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_LOGIN
    {
       
        public BigInteger Id {  get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
    }
}