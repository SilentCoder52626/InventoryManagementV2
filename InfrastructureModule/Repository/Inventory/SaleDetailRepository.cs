using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using InventoryLibrary.Entity;
using InventoryLibrary.Source.Repository.Interface;

namespace InventoryLibrary.Source.Repository
{
    public class SaleDetailRepository : BaseRepository<SaleDetails>, SaleDetailRepositoryInterface
    {
        public SaleDetailRepository(AppDbContext context) : base(context)
        {

        }
    }
}
