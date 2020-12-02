namespace DTXMania.Configuration.Settings
{
    /// <summary>
    /// A <see cref="string"/>-backed <see cref="ISetting{T}"/>.
    /// </summary>
    public class CStringSetting : ISetting<string>
    {
        public ESettingCategory eCategory { get; }

        public string strKey { get; }

        public CStringSetting(ESettingCategory eCategory, string strKey)
        {
            this.eCategory = eCategory;
            this.strKey = strKey;
        }

        public string tEncode(string value) => string.IsNullOrEmpty(value) ? string.Empty : value;

        public string tDecode(string strValue) => strValue;
    }
}
