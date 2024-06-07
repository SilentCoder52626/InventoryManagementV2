using Inventory.ViewModels.SaleDetail;
using InventoryLibrary.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.ViewModels.Sale
{
    public class SaleIndexViewModel
    {
        public long SaleId { get; set; }

        [Display(Name = "Customer")]
        public long CusId { get; set; }

        public string? CustomerName { get; set; }

        [Display(Name = "Item Name")]
        public long ItemId { get; set; }

        public string? ItemName { get; set; }

        [Display(Name = "Total ")]
        [Required]
        public long total { get; set; }

        public long discount { get; set; }

        public long netTotal { get; set; }
        public long paidAmount { get; set; }

        public long returnAmount { get; set; }
        public long dueAmount { get; set; }


        public DateTime date { get; set; }

        public List<SaleDetailIndexViewModel> SalesDetails { get; set; }
        public IList<Customer> customers { get; set; } = new List<Customer>();

        public SelectList CustomerSelectList =>
            new SelectList(customers, nameof(Customer.CusId), nameof(Customer.FullName));
        public InventoryLibrary.Entity.Item? item { get; set; }
        public SelectList itemList => new SelectList(
        ItemsWithDisplayText(),
        nameof(ItemWithDisplayText.Id),
        nameof(ItemWithDisplayText.DisplayText)
    );

        public IList<InventoryLibrary.Entity.Item> items { get; set; } = new List<InventoryLibrary.Entity.Item>();
        private IEnumerable<ItemWithDisplayText> ItemsWithDisplayText()
        {
            foreach (var item in items)
            {
                yield return new ItemWithDisplayText
                {
                    Id = item.Id,
                    DisplayText = $"{item.Name} per {item.Unit.Name}"
                };
            }
        }
    }
    public class ItemWithDisplayText
    {
        public long Id { get; set; }
        public string DisplayText { get; set; }
    }
}
