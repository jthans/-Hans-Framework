using Hans.Inventory.Core.Interfaces;
using System;

namespace Hans.Inventory.Core.Test.Models
{
    /// <summary>
    ///  Test Inventory Item.
    /// </summary>
    public class TestInventoryItem : IIInventoryItem
    {
        /// <summary>
        ///  ID of the Item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///  Instantiates a new instance of the <see cref="TestInventoryItem" /> class.  Will generate an ID upon creation.
        /// </summary>
        public TestInventoryItem()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
