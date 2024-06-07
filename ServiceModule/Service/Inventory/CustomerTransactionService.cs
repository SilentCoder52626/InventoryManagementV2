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
        private readonly CustomerTansactionRepositoryInterface _transactionRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerTransactionService(CustomerRepositoryInterface customerRepo, CustomerTansactionRepositoryInterface transactionRepo, IUnitOfWork unitOfWork)
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
            CustomerTransaction Transaction = new CustomerTransaction(customer, dto.Amount, dto.Type, dto.ExtraId);
            await _transactionRepo.InsertAsync(Transaction).ConfigureAwait(false);
        }
    }
}
