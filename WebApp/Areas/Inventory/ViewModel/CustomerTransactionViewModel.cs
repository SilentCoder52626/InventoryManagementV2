using Inventory.ViewModel;
using InventoryLibrary.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Inventory.ViewModels
{
    public class CustomerTransactionFilterModel : BaseFilterModel
    {
        public int CustomerId { get; set; }
    }
    public class CustomerTransactionViewModel
    {
        public long CustomerId { get; set; }
        public IList<Customer> customers { get; set; } = new List<Customer>();

        public SelectList CustomerSelectList =>
            new SelectList(customers, nameof(Customer.CusId), nameof(Customer.FullName));
    }
    public class CustomerTransactionModel
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionTime => TransactionDate.ToString("hh:mm tt");
        public string TransactionDateNepali{ get; set; }
        public long TransactionId { get; set; }
        public string AmountType { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string BalanceType { get; set; }

    }
}
