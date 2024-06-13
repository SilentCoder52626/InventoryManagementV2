using Inventory.ViewModels;
using InventoryLibrary.Entity;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Source.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Controllers
{
    [Area("inventory")]
    [Authorize]
    public class CustomerStatementController : Controller
    {
        CustomerTransactionRepositoryInterface _transactionRepo;
        CustomerRepositoryInterface _customerRepo;

        public CustomerStatementController(CustomerTransactionRepositoryInterface transactionRepo, CustomerRepositoryInterface customerRepo)
        {
            _transactionRepo = transactionRepo;
            _customerRepo = customerRepo;
        }

        public async Task<IActionResult> Index(long customerId = 0)
        {

            CustomerTransactionViewModel vm = new CustomerTransactionViewModel
            {
                CustomerId = customerId,
                customers = await _customerRepo.GetAllAsync()
            };
            if (customerId > 0)
            {
                var Customer = await _customerRepo.GetByIdAsync(customerId).ConfigureAwait(false);
                vm.CustomerName = Customer.FullName;

                var Transactions = await _transactionRepo.GetAllTransactionOfCustomer(customerId).ConfigureAwait(false);
                foreach (var t in Transactions)
                {
                    var tt = new CustomerTransactionModel()
                    {
                        TransactionDate = t.TransactionDate,
                        Amount = t.Amount,
                        AmountType = t.AmountType,
                        TransactionId = t.Id,
                        Type = t.Type,
                        BalanceAmount = Math.Abs(t.Balance),
                        BalanceType = t.Balance < 0 ? "" : "(Due)"
                    };

                    vm.Transactions.Add(tt);
                }
                vm.Transactions = vm.Transactions.OrderByDescending(a => a.TransactionDate).ToList();
            }
            return View(vm);
        }
    }
}
