namespace DTXMania.Configuration.Settings
{
    /// <summary>
    /// The different top-level categories that an <see cref="ISetting{T}"/> can be within.
    /// </summary>
    /// <remarks>
    /// Note that <see cref="ISetting{T}.strKey"/> must be unique within a category, but can be non-unique between different categories.
    /// </remarks>
    public enum ESettingCategory
    {
        /// <summary>
        /// System-related settings.
        /// </summary>
        System,

        /// <summary>
        /// Drum mode-related settings.
        /// </summary>
        Drum,

        /// <summary>
        /// Guitar mode-related settings.
        /// </summary>
        Guitar,
    }
}
