using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class DepartmentService:IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<TBL_DEPARTMENT> GetAllDepartments()
        {

            var dept = _unitOfWork.Department.GetAll();
            return (dept);
        }
     
    }
}
