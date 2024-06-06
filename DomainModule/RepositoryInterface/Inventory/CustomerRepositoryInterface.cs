using DomainModule.BaseRepo;
using InventoryLibrary.Entity;
using System.Threading.Tasks;

namespace InventoryLibrary.Repository.Interface
{
    public interface CustomerRepositoryInterface : BaseRepositoryInterface<Customer>
    {
        Task<Customer?> GetByNumber(string number);
    }
}
