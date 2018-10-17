using Hans.Inventory.Core.Exceptions;
using Hans.Inventory.Core.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Hans.Inventory.Core.Test
{
    /// <summary>
    ///  Tests the Inventory logic, to ensure everything exists/is modified correctly.
    /// </summary>
    [TestClass]
    public class InventoryTest
    {
        #region AddItem

        /// <summary>
        ///  Ensures an item can be added to a singular category inventory successfully.
        /// </summary>
        [TestMethod]
        public void AddItem_OneCategorySuccessfully()
        {
            // Create the test inventory, and test item.
            Inventory testInv = new Inventory();
            TestInventoryItem testItem = new TestInventoryItem();

            // Add the inventory, and confirm it exists in there.
            testInv.AddItem(testItem, 3);

            // Ensure the quantity was properly tracked in the system.
            var invStatus = testInv.GetItemProfile(testItem);

            Assert.AreEqual(invStatus.InventoryByCategory[0], 3);
            Assert.AreEqual(invStatus.TotalQuantity, 3);
        }

        /// <summary>
        ///  Ensures an item can be added to a category when multiple are managed.
        /// </summary>
        [TestMethod]
        public void AddItem_MultipleCategoriesSuccessfully()
        {
            // Create the test inventory, and test item.
            Inventory testInv = new Inventory(5);
            TestInventoryItem testItem = new TestInventoryItem();

            // Add the inventory, and confirm it exists in there.
            testInv.AddItem(testItem, 5, 3);
            testInv.AddItem(testItem, 7, 2);

            // Ensure the quantity was properly tracked in the system.
            var invStatus = testInv.GetItemProfile(testItem);

            Assert.AreEqual(invStatus.InventoryByCategory[3], 5);
            Assert.AreEqual(invStatus.InventoryByCategory[2], 7);
            Assert.AreEqual(invStatus.TotalQuantity, 12);
        }

        /// <summary>
        ///  Ensures that when no category is given, the proper exception is thrown.
        /// </summary>
        [TestMethod]
        public void AddItem_ThrowsExceptionWhenMultipleCategories()
        {
            // Create the test inventory, and test item.
            Inventory testInv = new Inventory(5);
            TestInventoryItem testItem = new TestInventoryItem();

            // Attempt to add the inventory.
            Assert.ThrowsException<ArgumentNullException>(() => testInv.AddItem(testItem, 4));
        }

        /// <summary>
        ///  Ensures that when a category doesn't exist in the inventory, the proper exception is thrown.
        /// </summary>
        [TestMethod]
        public void AddItem_ThrowsExceptionCategoryOutOfRange()
        {
            // Create the test inventory, and test item.
            Inventory testInv = new Inventory(5);
            TestInventoryItem testItem = new TestInventoryItem();

            // Attempt to add the inventory.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => testInv.AddItem(testItem, 4, 22));
        }

        #endregion

        #region RemoveItem

        /// <summary>
        ///  Ensures that when all quantity for an item is removed, the item is fully removed.
        /// </summary>
        [TestMethod]
        public void RemoveItem_RemovesItemEntirelySuccessfully()
        {
            // Create the test inventory, and test item.
            Inventory testInv = new Inventory(5);
            TestInventoryItem testItem = new TestInventoryItem();
            testInv.AddItem(testItem, 5, 3);
            testInv.AddItem(testItem, 7, 2);

            // Remove the item, and make sure it was removed.
            testInv.RemoveItem(testItem, 5, 3);
            var invStatus = testInv.GetItemProfile(testItem);

            // Ensure the category doesn't see it at all.
            Assert.IsFalse(invStatus.InventoryByCategory.ContainsKey(3));
        }

        /// <summary>
        ///  Ensures that when some but not all quantity is removed, it shows up and is correct.
        /// </summary>
        [TestMethod]
        public void RemoveItem_RemovesSomeQuantitySuccessfully()
        {
            // Create the test inventory, and test item.
            Inventory testInv = new Inventory(5);
            TestInventoryItem testItem = new TestInventoryItem();
            testInv.AddItem(testItem, 5, 3);
            testInv.AddItem(testItem, 7, 2);

            // Remove the item, and make sure it was removed.
            testInv.RemoveItem(testItem, 2, 3);
            var invStatus = testInv.GetItemProfile(testItem);

            Assert.AreEqual(3, invStatus.InventoryByCategory[3]);
        }

        /// <summary>
        ///  Ensures when a category is required and one is not passed, an exception is thrown.
        /// </summary>
        [TestMethod]
        public void RemoveItem_ThrowsExceptionMultipleCategories()
        {
            // Create the test inventory, and test item.
            Inventory testInv = new Inventory(5);
            TestInventoryItem testItem = new TestInventoryItem();

            // Attempt to remove the inventory.
            Assert.ThrowsException<ArgumentNullException>(() => testInv.RemoveItem(testItem, 4));
        }

        /// <summary>
        ///  Ensures that when an item is attempting to remove more than exists, an exception is thrown.
        /// </summary>
        [TestMethod]
        public void RemoveItem_ThrowsExceptionRemovingMoreQuantityThanExists()
        {
            // Create the test inventory, and test item.
            Inventory testInv = new Inventory(5);
            TestInventoryItem testItem = new TestInventoryItem();
            testInv.AddItem(testItem, 3, 3);

            // Attempt to remove the inventory.
            Assert.ThrowsException<InsufficientInventoryException>(() => testInv.RemoveItem(testItem, 4, 3));
        }

        /// <summary>
        ///  Ensures that when an item doesn't exist in the category, we throw an exception.
        /// </summary>
        [TestMethod]
        public void RemoveItem_ThrowsExceptionRemovingNonExistentItem()
        {
            // Create the test inventory, and test item.
            Inventory testInv = new Inventory(5);
            TestInventoryItem testItem = new TestInventoryItem();

            // Attempt to remove the inventory.
            Assert.ThrowsException<InsufficientInventoryException>(() => testInv.RemoveItem(testItem, 4, 3));
        }

        #endregion
    }
}
