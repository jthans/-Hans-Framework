using Hans.Inventory.Core.Interfaces;
using System;

namespace Hans.Inventory.Core.Models
{
    /// <summary>
    ///  Model class containing all pertinent information about a given item existing in inventory.
    /// </summary>
    public class InventoryElement
    {
        /// <summary>
        ///  Gets the ID of the item this element represents.
        /// </summary>
        public Guid? Id
        {
            get
            {
                return this.Item?.Id;
            }
        }

        /// <summary>
        ///  Gets or sets the item properties this element represents.
        /// </summary>
        public IIInventoryItem Item { get; set; }

        /// <summary>
        ///  Gets or sets the quantity of this item represented in the inventory.
        /// </summary>
        public int Quantity { get; set; }
    }
}
