using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using InventoryLibrary.Entity;
using InventoryLibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Repository.Implementation
{
    public class CustomerRepository : BaseRepository<Customer>, CustomerRepositoryInterface
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<Customer> GetByNumber(string number)
        {
            return await GetQueryable().SingleOrDefaultAsync(a => a.PhoneNumber == number);
        }
    }
}
