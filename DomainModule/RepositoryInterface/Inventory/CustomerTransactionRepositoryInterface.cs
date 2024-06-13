using DomainModule.BaseRepo;
using InventoryLibrary.Entity;

namespace InventoryLibrary.Source.Repository.Interface
{
    public interface CustomerTransactionRepositoryInterface : BaseRepositoryInterface<CustomerTransaction>
    {
        Task<List<CustomerTransaction>> GetAllTransactionOfCustomer(long customerId);
        decimal GetCustomerBalanceAmount(long customerId);
    }
}
