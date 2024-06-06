
using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using InventoryLibrary.Source.Entity;
using InventoryLibrary.Source.Repository.Interface;

namespace InventoryLibrary.Source.Repository
{
    public class PurchaseDetailRepository : BaseRepository<PurchaseDetail>, PurchaseDetailRepositoryInterface
    {
        public PurchaseDetailRepository(AppDbContext context) : base(context)
        {

        }
    }
}
