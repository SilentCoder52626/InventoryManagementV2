using System.Linq;
using System.Threading.Tasks;
using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using InventoryLibrary.Entity;
using InventoryLibrary.Repository.Interface;

namespace Inventory.Repository.Implementation
{

    public class ItemRepository : BaseRepository<Item>, ItemRepositoryInterface
    {
        public ItemRepository(AppDbContext context) : base(context)
        {

        }
        
    }
}
