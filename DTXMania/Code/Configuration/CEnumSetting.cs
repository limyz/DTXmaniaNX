using System;

namespace DTXMania
{
    /// <summary>
    /// An <see cref="Enum"/>-backed <see cref="ISetting{T}"/>.
    /// </summary>
    /// <typeparam name="T">The enumerated type of this setting's value.</typeparam>
    public class CEnumSetting<T> : ISetting<T> where T : Enum
    {
        public string strKey { get; }

        public CEnumSetting(string strKey)
        {
            this.strKey = strKey;
        }

        public string tEncode(T value) => value.ToString();

        public T tDecode(string strValue) => (T)Enum.Parse(typeof(T), strValue);
    }
}
