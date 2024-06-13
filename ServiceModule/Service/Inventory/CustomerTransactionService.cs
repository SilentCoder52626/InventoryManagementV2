using DomainModule.RepositoryInterface;
using InventoryLibrary.Entity;
using InventoryLibrary.Exceptions;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Source.Dto.CustomerTransaction;
using InventoryLibrary.Source.Repository.Interface;
using InventoryLibrary.Source.Services.ServiceInterface;
using System.Threading.Tasks;

namespace InventoryLibrary.Source.Services.Implementation
{
    public class CustomerTransactionService : CustomerTransactionServiceInterface
    {
        private readonly CustomerRepositoryInterface _customerRepo;
        private readonly CustomerTransactionRepositoryInterface _transactionRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerTransactionService(CustomerRepositoryInterface customerRepo, CustomerTransactionRepositoryInterface transactionRepo, IUnitOfWork unitOfWork)
        {
            _customerRepo = customerRepo;
            _transactionRepo = transactionRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(CustomerTransactionCreateDto dto)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    await CreateWithoutTransaction(dto).ConfigureAwait(false);
                    _unitOfWork.Complete();
                    tx.Commit();
                }
            }
        }

        public async Task CreateWithoutTransaction(CustomerTransactionCreateDto dto)
        {
            var customer = await _customerRepo.GetByIdAsync(dto.CustomerId).ConfigureAwait(false) ?? throw new CustomerNotFoundException();
            CustomerTransaction Transaction = new CustomerTransaction(customer, dto.Amount, dto.Type, dto.ExtraId)
            {
                Balance = dto.Balance
            };
            await _transactionRepo.InsertAsync(Transaction).ConfigureAwait(false);
        }

        public async Task ReCalculateCustomerBalance()
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {
                    var CustomerIds = _customerRepo.GetQueryable().Select(c => c.CusId).ToList();
                    foreach (var customerId in CustomerIds)
                    {
                        var Transactions = _transactionRepo.GetQueryable().Where(c => c.CustomerId == customerId).OrderBy(a=>a.TransactionDate).ToList();


                        
                        for (int i = 0; i < Transactions.Count; i++)
                        {
                            var CurrentData = Transactions[i];
                            var CurrentIndex = i;
                            int PreviousIndex = CurrentIndex - 1;
                            decimal PreviousBalance = 0;
                            
                            if (i > 0)
                            {
                                PreviousBalance = Transactions[PreviousIndex].Balance;
                            }

                            var BalanceAmount = CurrentData.AmountType == CustomerTransaction.TypeDebit
                                ? PreviousBalance + CurrentData.Amount
                                : PreviousBalance - CurrentData.Amount;
                            CurrentData.Balance =BalanceAmount;

                            await _transactionRepo.UpdateAsync(CurrentData).ConfigureAwait(false);
                        }

                    }
                    _unitOfWork.Complete();
                    tx.Commit();
                }
            }
        }
    }
}
