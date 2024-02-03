using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class DocumentRepository : Repository<TBL_DOCUMENT>, IDocumentRepository
    {
        public DocumentRepository(AppDbContext context) : base(context)
        {
        }
    }
}