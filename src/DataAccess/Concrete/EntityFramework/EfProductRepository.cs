using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductRepository: EfEntityRepositoryBase<Product, ProjectDbContext>, IProductRepository
    {
        public EfProductRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}