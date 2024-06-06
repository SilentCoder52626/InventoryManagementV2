using DomainModule.RepositoryInterface;
using InventoryLibrary.Dto.Purchase;
using InventoryLibrary.Entity;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Services.ServiceInterface;

using InventoryLibrary.Source.Repository.Interface;
using System;
using System.Threading.Tasks;

namespace InventoryLibrary.Source.Services.Implementation
{
    public class PurchaseService : PurchaseServiceInterface
    {
        private readonly PurchaseRepositoryInterface _purchaseRepo;
        private readonly SupplierRepositoryInterface _supplierRepo;
        private readonly ItemRepositoryInterface _itemRepo;
        private readonly IUnitOfWork _unitOfWork;
        public PurchaseService(
            PurchaseRepositoryInterface _purchaseRepo,
            SupplierRepositoryInterface _supplierRepo,
            ItemRepositoryInterface _itemRepo
,
            IUnitOfWork unitOfWork)
        {
            this._purchaseRepo = _purchaseRepo;
            this._supplierRepo = _supplierRepo;
            this._itemRepo = _itemRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task Create(PurchaseCreateDTO dto)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted)){

            var supplier = await _supplierRepo.GetByIdAsync(dto.SupplierId).ConfigureAwait(false) ?? throw new Exception("Supplier not found!!!");


            var purchase = new Purchase(supplier, dto.Discount);

            foreach (var data in dto.PurchaseDetails)
            {
                var item = await _itemRepo.GetByIdAsync(data.ItemId).ConfigureAwait(false) ?? throw new Exception("Item not found!");

                purchase.AddPurchaseDetails(item, data.Qty, data.Rate, data.SalesRate);
                item.AddQty(data.Qty);
                item.UpdateRate(data.SalesRate);
                await _itemRepo.UpdateAsync(item).ConfigureAwait(false);
            }

            await _purchaseRepo.InsertAsync(purchase).ConfigureAwait(false);



            tx.Commit();_unitOfWork.Complete();}

        }


    }
}
