using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using InventoryLibrary.Entity;
using InventoryLibrary.Source.Entity;
using InventoryLibrary.Source.Repository.Interface;

namespace InventoryLibrary.Source.Repository
{
    public class SaleRepository : BaseRepository<Sale>, SaleRepositoryInterface
    {
        public SaleRepository(AppDbContext context) : base(context)
        {

        }
    }
}
