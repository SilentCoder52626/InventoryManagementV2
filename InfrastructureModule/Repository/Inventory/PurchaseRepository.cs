using InfrastructureModule.Context;

using InfrastructureModule.Repository;
using InventoryLibrary.Entity;
using InventoryLibrary.Repository.Interface;

namespace InventoryLibrary.Source.Repository
{
    public class PurchaseRepository : BaseRepository<Purchase>, PurchaseRepositoryInterface
    {
        public PurchaseRepository(AppDbContext context) : base(context)
        {

        }
    }
}
