using System;

namespace DTXMania.Configuration.Settings
{
    /// <summary>
    /// An <see cref="Enum"/>-backed <see cref="ISetting{T}"/>.
    /// </summary>
    /// <typeparam name="T">The enumerated type of this setting's value.</typeparam>
    public class CEnumSetting<T> : ISetting<T> where T : Enum
    {
        public ESettingCategory eCategory { get; }

        public string strKey { get; }

        public CEnumSetting(ESettingCategory eCategory, string strKey)
        {
            this.eCategory = eCategory;
            this.strKey = strKey;
        }

        public string tEncode(T value) => value.ToString();

        public T tDecode(string strValue) => (T)Enum.Parse(typeof(T), strValue);
    }
}
