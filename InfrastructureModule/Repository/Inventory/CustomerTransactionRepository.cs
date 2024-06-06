using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using InventoryLibrary.Entity;
using InventoryLibrary.Source.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventoryLibrary.Source.Repository
{
    public class CustomerTransactionRepository : BaseRepository<CustomerTransaction>, CustomerTansactionRepositoryInterface
    {
        public CustomerTransactionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<CustomerTransaction>> GetAllTransactionOfCustomer(long customerId)
        {
            return await this.GetQueryable().Where(a => a.CustomerId == customerId).ToListAsync().ConfigureAwait(false);
        }
    }
}
