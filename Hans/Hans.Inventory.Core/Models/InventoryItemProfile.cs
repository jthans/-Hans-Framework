using System;
using System.Collections.Generic;
using System.Linq;

namespace Hans.Inventory.Core.Models
{
    /// <summary>
    ///  Model that contains information about the item inside the inventory.
    /// </summary>
    public class InventoryItemProfile
    {
        /// <summary>
        ///  Id of the item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///  Categories this item is contained in, and how many of the item.
        /// </summary>
        public Dictionary<int, int> InventoryByCategory { get; set; }

        /// <summary>
        ///  Returns the total quantity of an item in the inventory.
        /// </summary>
        public int TotalQuantity { get { return this.InventoryByCategory.Values.Sum();  } }
    }
}
