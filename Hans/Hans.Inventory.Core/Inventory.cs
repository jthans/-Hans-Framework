using Hans.Inventory.Core.Exceptions;
using Hans.Inventory.Core.Interfaces;
using Hans.Inventory.Core.Models;
using Hans.Logging;
using Hans.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hans.Inventory.Core
{
    /// <summary>
    ///  Inventory class, that handles all items contained within, giving access to adding/removing, and seeing what's inside, etc.  This
    ///     class can be drastically expanded to implement different types of inventories.
    /// </summary>
    public class Inventory
    {
        #region Fields

        /// <summary>
        ///  Gets or sets the internal inventory managed by this class - Is an array, as we can represent any number of categories desired in the
        ///     variable, or we can simply run off of a single inventory, if categorization/speed is not as critical.
        ///     For larger inventories, where many items can be a factor, looking at a particular category can speed up the process.
        /// </summary>
        Dictionary<Guid, InventoryElement>[] cachedInventory;

        /// <summary>
        ///  Creates a logger for this class, so we can output information about what's happening.
        /// </summary>
        ILogger log = LoggerManager.CreateLogger(typeof(Inventory));

        #endregion

        #region Constructors

        /// <summary>
        ///  Instantiates a new instance of the <see cref="Inventory" /> class, with an assigned size
        ///     of 1, meaning no catergories are generated.
        /// </summary>
        /// <param name="numCategories">The number of categories we'll be maintaining in this class.</param>
        public Inventory(int numCategories = 1)
        {
            // Creates this inventory with a size passed.
            log.LogMessage($"Inventory being created with { numCategories } categories available.");
            this.cachedInventory = new Dictionary<Guid, InventoryElement>[numCategories];
            for (var newCat = 0; newCat < numCategories; newCat++)
            {
                this.cachedInventory[newCat] = new Dictionary<Guid, InventoryElement>();
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Adds an item to the inventory collection, by first seeing if it already exists in the given category.
        /// </summary>
        /// <param name="itemToAdd">The item to add to the inventory.</param>
        /// <param name="qtyToAdd">How many of this item to add to the inventory.</param>
        /// <param name="categoryToAddTo">Which category to add this inventory to - REQUIRED if there's more than one collection.</param>
        /// <returns>How many of this item exist in the category requested after the action.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the inventory is set up to handle multiple categories, and the category was not given in the call.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the category requested is outside the bounds managed in the inventory.</exception>
        public int AddItem(IIInventoryItem itemToAdd, int qtyToAdd = 1, int? categoryToAddTo = null)
        {
            // Check our category input - We either need to assign a value, throw an exception, or continue, depending.
            categoryToAddTo = this.CalculateCategoryToUse(categoryToAddTo);

            // Actually modify the collection, creating the entry if necessary.
            if (!this.cachedInventory[categoryToAddTo.Value].ContainsKey(itemToAdd.Id))
            {
                this.CreateElementForItem(itemToAdd, categoryToAddTo.Value);
            }

            this.cachedInventory[categoryToAddTo.Value][itemToAdd.Id].Quantity += qtyToAdd;
            return this.cachedInventory[categoryToAddTo.Value][itemToAdd.Id].Quantity;
        }

        /// <summary>
        ///  Returns the item profile, explaining where and how the item is stored in the DB.
        /// </summary>
        /// <param name="itemSearch">The item we're searching.</param>
        /// <returns>The item profile, and how it's managed in the DB.</returns>
        public InventoryItemProfile GetItemProfile(IIInventoryItem itemSearch)
        {
            // Build the dictionary of category/quantity combos.
            Dictionary<int, int> categoryDic = new Dictionary<int, int>();
            for (var i = 0; i < this.cachedInventory.Length; i++)
            {
                // If this category has the item stored, add it with its quantity.
                if (this.cachedInventory[i].ContainsKey(itemSearch.Id))
                {
                    categoryDic.Add(i, this.cachedInventory[i][itemSearch.Id].Quantity);
                }
            }

            // Return the new profile we've calculated.
            return new InventoryItemProfile()
            {
                Id = itemSearch.Id,
                InventoryByCategory = categoryDic
            };
        }

        /// <summary>
        ///  Removes an item from the inventory collection, ensuring some actually does exist in the given category.
        /// </summary>
        /// <param name="itemToRemove">The item to remove from the inventory.</param>
        /// <param name="qtyToRemove">The quantity we're taking out of the inventory.</param>
        /// <param name="categoryToRemoveFrom">The category we're removing inventory from.</param>
        /// <returns>How many of this item exist in the category requested after the action.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the inventory is set up to handle multiple categories, and the category was not given in the call.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the category requested is outside the bounds managed in the inventory.</exception>
        public int RemoveItem(IIInventoryItem itemToRemove, int qtyToRemove = 1, int? categoryToRemoveFrom = null)
        {
            // Check the category input, to ensure it's valid.
            categoryToRemoveFrom = this.CalculateCategoryToUse(categoryToRemoveFrom);

            // Modify the collection, deleting the record if necessary.
            if (this.cachedInventory[categoryToRemoveFrom.Value].ContainsKey(itemToRemove.Id))
            {
                int qtyRemaining = this.cachedInventory[categoryToRemoveFrom.Value][itemToRemove.Id].Quantity - qtyToRemove;
                if (qtyRemaining < 0)
                {
                    // We didn't have enough quantity to remove this quantity.
                    throw new InsufficientInventoryException(qtyToRemove, itemToRemove);
                }
                else if (qtyRemaining == 0)
                {
                    // We need to actually remove this entry from the cache, we deleted it.
                    this.cachedInventory[categoryToRemoveFrom.Value].Remove(itemToRemove.Id);
                }
                else
                {
                    // Subtract the requested inventory from the inventory cache.
                    this.cachedInventory[categoryToRemoveFrom.Value][itemToRemove.Id].Quantity -= qtyToRemove;
                }

                // Return the number remaining in the location.
                return qtyRemaining;
            }
            else
            {
                throw new InsufficientInventoryException(qtyToRemove, itemToRemove);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        ///  Calculates the category to use in the inventory, checking the size of the container to make sure it's within range.
        /// </summary>
        /// <param name="categoryNum">The category we're trying to access.</param>
        /// <returns>The category to access for this logic.</returns>
        private int? CalculateCategoryToUse(int? categoryNum)
        {
            // Check our category input - We either need to assign a value, throw an exception, or continue, depending.
            if (!categoryNum.HasValue)
            {
                // If we're managing more than 1 category, we must know which category to modify.
                if (this.cachedInventory.Length > 1) { throw new ArgumentNullException("categoryNum", $"Inventory is set up to manage { this.cachedInventory.Length } categories, selection must be passed."); }
                categoryNum = 0; // We're only managing one - Modify the default collection.
            }
            else if (categoryNum.Value < 0 ||
                        categoryNum.Value > this.cachedInventory.Length)
            {
                throw new ArgumentOutOfRangeException("categoryNum", $"Inventory has categories 0 - { this.cachedInventory.Length }.  Attempted to access category { categoryNum.Value }, out of range.");
            }

            return categoryNum;
        }

        /// <summary>
        ///  Creates an element in the given category for the item requested in our inventory.  This will be an empty element, with qty 0.
        /// </summary>
        /// <param name="itemToCreate">The item to creat in our inventory.</param>
        /// <param name="categoryNum">The category to add the item to.</param>
        private void CreateElementForItem(IIInventoryItem itemToCreate, int categoryNum)
        {
            // Don't create it, if it already exists.
            if (this.cachedInventory[categoryNum].ContainsKey(itemToCreate.Id)) { return; }
            this.cachedInventory[categoryNum].Add(itemToCreate.Id, new InventoryElement() { Item = itemToCreate, Quantity = 0 });
        }

        #endregion
    }
}
