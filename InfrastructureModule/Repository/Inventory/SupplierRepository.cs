using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using InventoryLibrary.Entity;
using InventoryLibrary.Source.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryLibrary.Source.Repository
{
    public class SupplierRepository : BaseRepository<Supplier>, SupplierRepositoryInterface
    {
        public SupplierRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Supplier> GetByNumber(string number)
        {
            return await this.GetQueryable().SingleOrDefaultAsync(a => a.Phone == number);
        }
    }
}
