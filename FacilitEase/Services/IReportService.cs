using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IReportService
    {
        Report GetReportData(int id);

        ChartData GetChartData(int id);

        ProfileData GetProfileData(int id);
    }
}