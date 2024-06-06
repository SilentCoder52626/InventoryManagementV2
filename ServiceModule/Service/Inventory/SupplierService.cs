using DomainModule.RepositoryInterface;
using InventoryLibrary.Dto.Supplier;
using InventoryLibrary.Entity;
using InventoryLibrary.Services.ServiceInterface;
using InventoryLibrary.Source.Dto.Supplier;
using InventoryLibrary.Source.Repository.Interface;
using System;
using System.Threading.Tasks;


namespace InventoryLibrary.Services.Implementation
{
    public class SupplierService : SupplierServiceInterface
    {
        private readonly SupplierRepositoryInterface _supplierRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(SupplierRepositoryInterface _supplierRepo, IUnitOfWork unitOfWork)
        {
            this._supplierRepo = _supplierRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Activate(long id)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    var supplier = await _supplierRepo.GetByIdAsync(id).ConfigureAwait(false) ?? throw new System.Exception("Supplier Not Found.");

                    supplier.Enable();

                    await _supplierRepo.UpdateAsync(supplier).ConfigureAwait(false);
                    tx.Commit();
                    _unitOfWork.Complete();
                }
            }
        }

        public async Task Create(SupplierCreateDTO dto)
        {

            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    await ValidateSupplierNumber(dto.Phone);
                    var supplier = new Supplier(dto.Name, dto.Address, dto.Email, dto.Phone);

                    await _supplierRepo.InsertAsync(supplier).ConfigureAwait(false);
                    tx.Commit();
                    _unitOfWork.Complete();
                }

            }
        }
        private async Task ValidateSupplierNumber(string phoneNumber, Supplier? supplier = null)
        {
            var CustomerByNumber = await _supplierRepo.GetByNumber(phoneNumber).ConfigureAwait(false);
            if (CustomerByNumber != null && CustomerByNumber != supplier)
                throw new Exception("Supplier Number already registered.");
            return;

        }

        public async Task Deactivate(long id)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    var supplier = await _supplierRepo.GetByIdAsync(id).ConfigureAwait(false) ?? throw new System.Exception("Supplier Not Found.");

                    supplier.Disable();

                    await _supplierRepo.UpdateAsync(supplier).ConfigureAwait(false);
                    tx.Commit();
                    _unitOfWork.Complete();
                }
            }
        }


        public async Task Update(SupplierUpdateDTO dto)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    var supplier = await _supplierRepo.GetByIdAsync(dto.Id).ConfigureAwait(false) ?? throw new System.Exception("Supplier Not Found.");
                    await ValidateSupplierNumber(dto.Phone, supplier);
                    supplier.Update(dto.Name, dto.Address, dto.Email, dto.Phone);

                    await _supplierRepo.UpdateAsync(supplier);
                    tx.Commit();
                    _unitOfWork.Complete();
                }
            }
        }
    }
}
