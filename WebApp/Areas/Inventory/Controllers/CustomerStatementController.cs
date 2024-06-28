using Castle.Core.Resource;
using Inventory.ViewModels;
using InventoryLibrary.Entity;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Source.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NepaliDateConverter;
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
        IDateConverterService _dateConverter;

        public CustomerStatementController(CustomerTransactionRepositoryInterface transactionRepo, CustomerRepositoryInterface customerRepo, IDateConverterService dateConverter)
        {
            _transactionRepo = transactionRepo;
            _customerRepo = customerRepo;
            _dateConverter = dateConverter;
        }

        public async Task<IActionResult> Index(long customerId = 0)
        {

            CustomerTransactionViewModel vm = new CustomerTransactionViewModel
            {
                CustomerId = customerId,
                customers = await _customerRepo.GetAllAsync()
            };
            return View(vm);
        }
        public async Task<IActionResult> LoadStatement(CustomerTransactionFilterModel model)
        {
            var TransactionQuery = _transactionRepo.GetQueryable().Where(a => a.CustomerId == model.CustomerId);

            var TotalCount = TransactionQuery.Count();

            var Transactions = TransactionQuery.OrderByDescending(a => a.TransactionDate).Skip(model.start).Take(model.length).ToList();
            var response = new List<CustomerTransactionModel>();
            foreach (var t in Transactions)
            {
                response.Add(new CustomerTransactionModel()
                {
                    TransactionDate = t.TransactionDate,
                    TransactionDateNepali = _dateConverter.ToBS(t.TransactionDate).ToString(),
                    Amount = t.Amount,
                    AmountType = t.AmountType,
                    TransactionId = t.Id,
                    Type = t.Type,
                    BalanceAmount = Math.Abs(t.Balance),
                    BalanceType = t.Balance < 0 ? "" : "(Due)"
                });                
            }
            var jsonData = new { draw = model.draw, recordsFiltered = TotalCount , recordsTotal = TotalCount, data = response };
            return Ok(jsonData);
        }
    }
}
