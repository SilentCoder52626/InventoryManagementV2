using Inventory.ViewModels;
using InventoryLibrary.Entity;
using InventoryLibrary.Exceptions;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Services.ServiceInterface;
using InventoryLibrary.Source.Dto.CustomerTransaction;
using InventoryLibrary.Source.DTO.Customer;
using InventoryLibrary.Source.Entity;
using InventoryLibrary.Source.Repository.Interface;
using InventoryLibrary.Source.Services.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory.Controllers
{
    [Area("inventory")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly CustomerServiceInterface _customerService;
        private readonly IToastNotification _toastNotification;
        private readonly CustomerRepositoryInterface _customerRepo;
        private readonly CustomerTransactionRepositoryInterface _transactionRepo;
        private readonly CustomerTransactionServiceInterface _transactionService;

        public CustomerController(IToastNotification toastNotification,
                                    CustomerServiceInterface _customerService,
                                    CustomerRepositoryInterface _customerRepo,
                                    SaleServiceInterface _saleService,
                                    CustomerTransactionRepositoryInterface transactionRepo,
                                    CustomerTransactionServiceInterface transactionService)
        {
            _toastNotification = toastNotification;
            this._customerService = _customerService;
            this._customerRepo = _customerRepo;
            _transactionRepo = transactionRepo;
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var Customer = await _customerRepo.GetQueryable().ToListAsync();
                var indexViewModel = new List<CustomerIndexViewModel>();

                foreach (var data in Customer)
                {

                    var model = new CustomerIndexViewModel
                    {
                        CusId = data.CusId,
                        PhoneNumber = data.PhoneNumber,
                        FullName = data.FullName,
                        Address = data.Address,
                        Email = data.Email,
                        Gender = data.Gender
                    };


                    indexViewModel.Add(model);
                }

                return View(indexViewModel);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
            }
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var customer = new CustomerCreateDTO
                    {
                        PhoneNumber = model.PhoneNumber,
                        FullName = model.FullName,
                        Email = model.Email,
                        Gender = model.Gender,
                        Address = model.Address
                    };


                    await _customerService.Create(customer).ConfigureAwait(true);
                    _toastNotification.AddSuccessToastMessage("Created:- " + customer.FullName);

                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> Update(long id)
        {

            try
            {
                var customer = await _customerRepo.GetByIdAsync(id).ConfigureAwait(true) ?? throw new CustomerNotFoundException("Customer has not been found of that number!");

                var model = new CustomerUpdateViewModel
                {
                    CusId = customer.CusId,
                    FullName = customer.FullName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Gender = customer.Gender,
                    Address = customer.Address
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return RedirectToAction("Index");

            }

        }
        [HttpPost]
        public async Task<IActionResult> Update(CustomerUpdateViewModel model)
        {
            try
            {
                var customer = new CustomerUpdateDTO
                {
                    CusId = model.CusId,
                    FullName = model.FullName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Email = model.Email
                };


                await _customerService.Update(customer).ConfigureAwait(true);

                _toastNotification.AddSuccessToastMessage("Updated to :- " + customer.FullName);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Payment(long id)
        {

            try
            {
                var customer = await _customerRepo.GetByIdAsync(id).ConfigureAwait(true) ?? throw new CustomerNotFoundException("Customer not found.");
                var CurrentBalance = _transactionRepo.GetCustomerBalanceAmount(customer.CusId);
                var model = new PaymentViewModel
                {
                    CusId = customer.CusId,
                    FullName = customer.FullName,
                   Amount = 0,
                   DueAmount = CurrentBalance > 0 ? CurrentBalance : 0,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return RedirectToAction("Index");

            }

        }

        [HttpPost]
        public async Task<IActionResult> Payment(PaymentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    if (!ModelState.IsValid)
                    {
                        var validationErrors = ModelState.SelectMany(error => error.Value.Errors.Select(message => message.ErrorMessage)).ToList();
                        string responseMessage = string.Join("</br>", validationErrors);
                        _toastNotification.AddErrorToastMessage(responseMessage);
                    }
                }
                var customer = await _customerRepo.GetByIdAsync(model.CusId).ConfigureAwait(true) ?? throw new CustomerNotFoundException("Customer not found.");
                var CurrentBalance = _transactionRepo.GetCustomerBalanceAmount(customer.CusId);

                // Type payment
                if (model.Amount > 0)
                {
                    await _transactionService.Create(new CustomerTransactionCreateDto()
                    {
                        CustomerId = customer.CusId,
                        Amount = model.Amount,
                        ExtraId = customer.CusId,
                        Type = CustomerTransaction.TypePayment,
                        Balance = CurrentBalance - model.Amount
                    }).ConfigureAwait(false);
                }
                _toastNotification.AddSuccessToastMessage("Customer Payment recorded.");
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
            }

            return RedirectToAction("Index");
        }

    }
}
