using DomainModule.BaseRepo;
using InventoryLibrary.Source.Entity;

namespace InventoryLibrary.Source.Repository.Interface
{
    public interface UnitRepositoryInterface : BaseRepositoryInterface<Unit>
    {
        Task<Unit> GetByName(string name);
    }
}
