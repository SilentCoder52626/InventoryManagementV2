﻿using Inventory.ViewModels.PurchaseDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventory.ViewModels.Sale;

namespace Inventory.ViewModels.Purchase
{
    public class PurchaseIndexViewModel
    {
        [Display(Name = "Purchase ID")]
        public long Id { get; set; }

        [Display(Name = "Supplier")]
        public long SupplierId { get; set; }

        [Display(Name = "Supplier Name")]
        public string? SupplierName { get; set; }
        [Display(Name = "Purchase Date")]
        public DateTime Date { get; set; }
        public string? NepaliDate { get; set; }

        [Display(Name = "Item")]
        public long ItemId { get; set; }

        [Display(Name = "Item Name ")]
        public string? ItemName { get; set; }

        [Display(Name = "Total ")]
        public decimal Total { get; set; }

        [Display(Name = "Grand Total")]
        public decimal GrandTotal { get; set; }

        [Display(Name = "Discount ")]
        public decimal Discount { get; set; }

        [Display(Name = "Remarks ")]
        public string? Remarks { get; set; }
        public List<PurchaseDetailCreateViewModel>? PurchaseDetails { get; set; }

        public IList<InventoryLibrary.Entity.Supplier> Suppliers { get; set; } =
            new List<InventoryLibrary.Entity.Supplier>();

        public SelectList SupplierSelectList => new SelectList(Suppliers, nameof(InventoryLibrary.Entity.Supplier.Id),
            nameof(InventoryLibrary.Entity.Supplier.Name));

        public IList<InventoryLibrary.Entity.Item>? Items { get; set; } = new List<InventoryLibrary.Entity.Item>();


        private IEnumerable<ItemWithDisplayText> ItemsWithDisplayText()
        {
            foreach (var item in Items)
            {
                yield return new ItemWithDisplayText
                {
                    Id = item.Id,
                    DisplayText = $"{item.Name} per {item.Unit.Name}"
                };
            }
        }
        public SelectList ItemSelectList => new SelectList(
ItemsWithDisplayText(),
nameof(ItemWithDisplayText.Id),
nameof(ItemWithDisplayText.DisplayText)
);
    }
}
