using Inventory.ViewModels.SalesReport;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Source.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NepaliDateConverter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Controllers
{
    [Area("inventory")]
    [Authorize]
    public class SalesReportController : Controller
    {
        SaleRepositoryInterface _salesRepo;
        CustomerRepositoryInterface _customerRepo;
        IDateConverterService _dateConverter;
        public SalesReportController(SaleRepositoryInterface salesRepo, CustomerRepositoryInterface customerRepo, IDateConverterService dateConverter)
        {
            _salesRepo = salesRepo;
            _customerRepo = customerRepo;
            _dateConverter = dateConverter;
        }

        public async Task<IActionResult> Index()
        {
            var Sales = await _salesRepo.GetAllAsync();
            var vm = new List<SalesReportViewModel>();
            foreach (var sale in Sales)
            {
                vm.Add(new SalesReportViewModel()
                {
                    Amount = sale.total,
                    GrandAmount = sale.netTotal,
                    DueAmount = sale.dueAmount,
                    Discount = sale.discount,
                    SalesId = sale.SaleId,
                    CustomerName = sale.customer.FullName,
                    PaidAmount = sale.paidAmount,
                    ReturnAmount = sale.returnAmount,
                    TransactionDate = sale.SalesDate,
                    TransactionDateNepali = _dateConverter.ToBS(sale.SalesDate).ToString(),
                });
            }
            return View(vm);
        }
    }
}
