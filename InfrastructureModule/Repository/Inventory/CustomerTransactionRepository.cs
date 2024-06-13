using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using InventoryLibrary.Entity;
using InventoryLibrary.Source.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventoryLibrary.Source.Repository
{
    public class CustomerTransactionRepository : BaseRepository<CustomerTransaction>, CustomerTransactionRepositoryInterface
    {
        public CustomerTransactionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<CustomerTransaction>> GetAllTransactionOfCustomer(long customerId)
        {
            return await this.GetQueryable().Where(a => a.CustomerId == customerId).OrderByDescending(a => a.TransactionDate).ToListAsync().ConfigureAwait(false);
        }

        public decimal GetCustomerBalanceAmount(long customerId)
        {
            var LastTransaction = this.GetQueryable().Where(a => a.CustomerId == customerId).OrderByDescending(a=>a.TransactionDate).FirstOrDefault();
            return LastTransaction == null ? 0 : LastTransaction.Balance;
        }
    }
}
