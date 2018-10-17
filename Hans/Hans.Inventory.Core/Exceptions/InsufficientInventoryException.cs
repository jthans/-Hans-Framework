using Hans.Inventory.Core.Interfaces;
using System;

namespace Hans.Inventory.Core.Exceptions
{
    /// <summary>
    ///  Exception that indicates there isn't sufficient inventory to perform a particular action.
    /// </summary>
    public class InsufficientInventoryException : Exception
    {
        #region Properties

        /// <summary>
        ///  The item name that was requested.
        /// </summary>
        public string ItemName = "";

        /// <summary>
        ///  The quantity that was requested in this action.
        /// </summary>
        public int Quantity = 0;

        #endregion

        #region Constructors

        /// <summary>
        ///  Instantiates a new instance of the <see cref="InsufficientInventoryException" /> class.  
        /// </summary>
        /// <param name="qtyRequested">The quantity that was requested in this action.</param>
        /// <param name="itemRequested">The item was requested in this action.</param>
        public InsufficientInventoryException(int qtyRequested, IIInventoryItem itemRequested)
        {
            this.ItemName = itemRequested.ToString().ToString();
            this.Quantity = qtyRequested;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///  Returns a string representation of the request that failed.
        /// </summary>
        /// <returns>The string representation of the exception.</returns>
        public override string ToString()
        {
            return $"Inventory not available for item [{ this.ItemName }], quantity [{ this.Quantity }].";
        }

        #endregion
    }
}
