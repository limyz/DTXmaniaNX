namespace DTXMania.Configuration.Settings
{
    /// <summary>
    /// A <see cref="string"/>-backed <see cref="ISetting{T}"/>.
    /// </summary>
    public class CStringSetting : ISetting<string>
    {
        public string strKey { get; }

        public CStringSetting(string strKey)
        {
            this.strKey = strKey;
        }

        public string tEncode(string value) => string.IsNullOrEmpty(value) ? string.Empty : value;

        public string tDecode(string strValue) => strValue;
    }
}
