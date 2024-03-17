using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ISLAService
    {
        public List<ShowSLAInfo> GetSLAInfo(int userid);

        public void EditSLA(int departmentId, int priorityId, int time);

        public DateTime GetTicketSLA(int ticketId);

        public void EditTicketSLA(int ticketId, int time);

    }
}