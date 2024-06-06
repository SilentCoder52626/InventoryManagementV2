using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using InventoryLibrary.Source.Entity;
using InventoryLibrary.Source.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryLibrary.Source.Repository
{
    public class UnitRepository : BaseRepository<Unit>, UnitRepositoryInterface
    {
        public UnitRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Unit> GetByName(string name)
        {
            return await this.GetQueryable().Where(a => a.Name == name).SingleOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
