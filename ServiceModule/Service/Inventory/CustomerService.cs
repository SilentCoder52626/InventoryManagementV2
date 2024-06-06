
using DomainModule.RepositoryInterface;
using InventoryLibrary.Entity;
using InventoryLibrary.Exceptions;
using InventoryLibrary.Repository.Interface;
using InventoryLibrary.Source.DTO.Customer;
using InventoryLibrary.Source.Services.ServiceInterface;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace InventoryLibrary.Source.Services
{
    public class CustomerService : CustomerServiceInterface
    {
        private readonly CustomerRepositoryInterface _customerRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(CustomerRepositoryInterface _customerRepo, IUnitOfWork unitOfWork)
        {
            this._customerRepo = _customerRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(CustomerCreateDTO dto)
        {
            using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
            {
                {

                    await ValidateCustomerNumber(dto.PhoneNumber);
                    var customer = new Customer(dto.FullName);
                    customer.Email = dto.Email;
                    customer.Address = dto.Address;
                    customer.PhoneNumber = dto.PhoneNumber;
                    customer.Gender = dto.Gender;


                    await _customerRepo.InsertAsync(customer);
                    tx.Commit();
                    _unitOfWork.Complete();
                }


            }
        }

            private async Task ValidateCustomerNumber(string phoneNumber, Customer? customer = null)
            {
                var CustomerByNumber = await _customerRepo.GetByNumber(phoneNumber).ConfigureAwait(false);
                if (CustomerByNumber != null && CustomerByNumber != customer)
                    throw new Exception("Customer Number already registered.");
                return;
            }

            public async Task Update(CustomerUpdateDTO dto)
            {
                using (var tx = await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
                {

                    var customer = await _customerRepo.GetByIdAsync(dto.CusId).ConfigureAwait(false);
                    await ValidateCustomerNumber(dto.PhoneNumber, customer).ConfigureAwait(false);
                    customer.Update(dto.FullName);
                    customer.Email = dto.Email;
                    customer.PhoneNumber = dto.PhoneNumber;
                    customer.Gender = dto.Gender;
                    customer.Address = dto.Address;

                    await _customerRepo.UpdateAsync(customer);

                    tx.Commit(); _unitOfWork.Complete();
                }

            }


        }
    }