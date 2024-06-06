using DomainModule.BaseRepo;
using InventoryLibrary.Entity;

namespace InventoryLibrary.Source.Repository.Interface
{
    public interface CustomerTansactionRepositoryInterface : BaseRepositoryInterface<CustomerTransaction>
    {
        Task<List<CustomerTransaction>> GetAllTransactionOfCustomer(long customerId);
    }
}
