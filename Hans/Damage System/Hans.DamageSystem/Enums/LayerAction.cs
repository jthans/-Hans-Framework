namespace Hans.DamageSystem.Enums
{
    /// <summary>
    ///  Used when processing layers in the <see cref="DamageController{T}" />, we want to know what to do with
    ///     a particular layer once it has been processed.
    /// </summary>
    public enum LayerAction
    {
        // Do Nothing.
        None,

        // Update the Layer.
        Update,

        // Remove the Layer.
        Remove
    }
}
