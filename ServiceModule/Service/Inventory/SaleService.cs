using DomainModule.RepositoryInterface;
using InventoryLibrary.Dto.Sale;
using InventoryLibrary.Entity;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Services.ServiceInterface;
using InventoryLibrary.Source.Dto.CustomerTransaction;
using InventoryLibrary.Source.Entity;

using InventoryLibrary.Source.Repository.Interface;
using InventoryLibrary.Source.Services.ServiceInterface;
using System;
using System.Threading.Tasks;

namespace InventoryLibrary.Services.Implementation
{
    public class SaleService : SaleServiceInterface
    {
        private readonly SaleRepositoryInterface _saleRepo;
        private readonly ItemRepositoryInterface _itemRepo;
        private readonly SaleDetailRepositoryInterface _saleDetailRepo;
        private readonly CustomerTransactionServiceInterface _transactionService;
        private readonly CustomerRepositoryInterface _customerRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomerTransactionRepositoryInterface _transactionRepo;
        public SaleService(SaleRepositoryInterface _saleRepo, SaleDetailRepositoryInterface _saleDetailRepo, ItemRepositoryInterface itemRepo, CustomerTransactionServiceInterface transactionService, CustomerRepositoryInterface customerRepo, IUnitOfWork unitOfWork, CustomerTransactionRepositoryInterface transactionRepo)
        {
            this._saleRepo = _saleRepo;
            this._saleDetailRepo = _saleDetailRepo;
            _itemRepo = itemRepo;
            _transactionService = transactionService;
            _customerRepo = customerRepo;
            _unitOfWork = unitOfWork;
            _transactionRepo = transactionRepo;
        }
        public async Task<Sale> Create(SaleCreateDTO dto)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                var Customer = await _customerRepo.GetByIdAsync(dto.CusId).ConfigureAwait(false);
                var sale = new Sale
                {
                    customer = Customer,
                    total = dto.total,
                    CusId = dto.CusId,
                    netTotal = dto.netTotal,
                    discount = dto.discount,
                    SalesDate = DateTime.Now,
                    paidAmount = dto.paidAmount,
                    returnAmount = dto.returnAmount,
                    dueAmount = Math.Abs(dto.paidAmount - (dto.netTotal + dto.returnAmount))
                };


                await _saleRepo.InsertAsync(sale).ConfigureAwait(false);
                _unitOfWork.Complete();


                foreach (var SaleData in dto.SaleDetails)
                {
                    var item = await _itemRepo.GetByIdAsync(SaleData.ItemId);

                    var SaleDetail = new SaleDetails
                    {
                        Qty = SaleData.Qty,
                        Total = SaleData.Total,
                        Price = SaleData.Price,
                        SaleId = sale.SaleId,
                        ItemName = item.Name,
                        UnitName = item.Unit.Name,
                    };
                    item.DecreaseQty(SaleData.Qty);
                    await _itemRepo.UpdateAsync(item);
                    await _saleDetailRepo.InsertAsync(SaleDetail).ConfigureAwait(false);
                }
                var CurrentBalance = _transactionRepo.GetCustomerBalanceAmount(sale.CusId);
                //Type Sales
                await _transactionService.CreateWithoutTransaction(new CustomerTransactionCreateDto()
                {
                    CustomerId = sale.CusId,
                    Amount = sale.netTotal,
                    ExtraId = sale.SaleId,
                    Type = CustomerTransaction.TypeSales,
                    Balance = CurrentBalance+sale.netTotal
                }).ConfigureAwait(false);

                CurrentBalance += (decimal)sale.netTotal;
                // Type payment
                if (sale.paidAmount - sale.returnAmount > 0)
                {
                    await _transactionService.CreateWithoutTransaction(new CustomerTransactionCreateDto()
                    {
                        CustomerId = sale.CusId,
                        Amount = sale.paidAmount - sale.returnAmount,
                        ExtraId = sale.SaleId,
                        Type = CustomerTransaction.TypePayment,
                        Balance = CurrentBalance - (sale.paidAmount - sale.returnAmount)
                    }).ConfigureAwait(false);
                }

                 _unitOfWork.Complete();
                tx.Commit();
                return sale;

            }
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();

        }

        public Task Update(SaleUpdateDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
