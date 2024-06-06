using DomainModule.RepositoryInterface;
using InventoryLibrary.Entity;
using InventoryLibrary.Exceptions;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Source.DTO.Item;
using InventoryLibrary.Source.Services.ServiceInterface;
using System.Threading.Tasks;

namespace InventoryLibrary.Source.Services
{
    public class ItemService : ItemServiceInterface
    {
        private readonly ItemRepositoryInterface _itemRepo;
        private readonly IUnitOfWork _unitOfWork;


        public ItemService(ItemRepositoryInterface _itemRepo, IUnitOfWork unitOfWork)
        {
            this._itemRepo = _itemRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Activate(long id)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    var item = await _itemRepo.GetByIdAsync(id).ConfigureAwait(false);

                    item.Enable();

                    await _itemRepo.UpdateAsync(item).ConfigureAwait(false);


                    _unitOfWork.Complete();
                    tx.Commit();
                }

            }
        }

        public async Task Create(ItemCreateDTO dto)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    var item = new Item(dto.Unit, dto.ItemName, dto.Price);
                    await _itemRepo.InsertAsync(item);
                    tx.Commit();
                    _unitOfWork.Complete();
                }

            }
        }

        public async Task Deactivate(long id)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    var item = await _itemRepo.GetByIdAsync(id).ConfigureAwait(false);
                    item.Disable();
                    await _itemRepo.UpdateAsync(item).ConfigureAwait(false);
                    tx.Commit();
                    _unitOfWork.Complete();
                }
            }
        }



        public async Task Delete(long id)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    var item = await _itemRepo.GetByIdAsync(id).ConfigureAwait(false);

                    await _itemRepo.DeleteAsync(item);
                    tx.Commit();
                    _unitOfWork.Complete();
                }

            }
        }

        public async Task Update(ItemUpdateDTO dto)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    var item = await _itemRepo.GetByIdAsync(dto.ItemId).ConfigureAwait(false) ?? throw new ItemNotFoundException();
                    item.Update(dto.Unit, dto.Name, dto.Price);
                    await _itemRepo.UpdateAsync(item);
                    tx.Commit();
                    _unitOfWork.Complete();
                }
            }
        }
    }
}

