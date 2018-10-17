using System;

namespace Hans.Inventory.Core.Interfaces
{
    /// <summary>
    ///  An item that resides in an inventory, with information necessary for 
    /// </summary>
    public interface IIInventoryItem
    {
        /// <summary>
        ///  Gets or sets the ID of the item, which will tell us which item it is.
        /// </summary>
        Guid Id { get; set; }
    }
}
