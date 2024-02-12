﻿using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ISLAService
    {
        public List<SLAInfo> GetSLAInfo(int userid);
        public void EditSLA(int departmentId, int priorityId, int time);
    }
}