using Inventory.ViewModels;
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
            var filterModel = new SalesReportFilterModel();
            filterModel.From = DateTime.Now.AddDays(-30);
            filterModel.To = DateTime.Now;
            return View(filterModel);
        }
        public async Task<IActionResult> LoadReport(SalesReportFilterModel model)
        {
            var SalesQuery = _salesRepo.GetQueryable();


            SalesQuery = SalesQuery.Where(c=>c.SalesDate >= model.From &&  c.SalesDate <= model.To);
            var TotalCount = SalesQuery.Count();


            var Sales = SalesQuery.OrderByDescending(a => a.SalesDate).Skip(model.start).Take(model.length).ToList();

            var response = new List<SalesReportViewModel>();
            foreach (var sale in Sales)
            {
                response.Add(new SalesReportViewModel()
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

            var jsonData = new { draw = model.draw, recordsFiltered = TotalCount, recordsTotal = TotalCount, data = response };
            return Ok(jsonData);
        }
    }
}
