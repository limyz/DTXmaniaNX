using DTXMania.Configuration;

namespace DTXMania.Configuration.Settings
{
    /// <summary>
    /// A keyed value which can be get and set within a <see cref="CConfigurationStore"/>.
    /// </summary>
    /// <typeparam name="T">The type of this setting's value.</typeparam>
    public interface ISetting<T>
    {
        /// <summary>
        /// The category that this setting is within.
        /// </summary>
        ESettingCategory eCategory { get; }

        /// <summary>
        /// The unique internal identifier of this setting.
        /// </summary>
        string strKey { get; }

        /// <summary>
        /// Encode the given <typeparamref name="T"/> into its <see cref="string"/> representation.
        /// </summary>
        /// <param name="value">The <typeparamref name="T"/> to encode.</param>
        /// <returns>The <see cref="string"/> representation of <paramref name="value"/>.</returns>
        string tEncode(T value);

        /// <summary>
        /// Decode the given <typeparamref name="T"/> <see cref="string"/> representation into its <typeparamref name="T"/> representation.
        /// </summary>
        /// <param name="strValue">The <see cref="string"/> to decode.</param>
        /// <returns>The <typeparamref name="T"/> representation of <paramref name="strValue"/>.</returns>
        T tDecode(string strValue);
    }
}
