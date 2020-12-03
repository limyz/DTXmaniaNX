namespace DTXMania.Configuration.Settings
{
    /// <summary>
    /// An <see cref="ISetting{T}"/> with a <see cref="bool"/> value.
    /// </summary>
    public class CBoolSetting : ISetting<bool>
    {
        public ESettingCategory eCategory { get; }

        public string strKey { get; }

        public CBoolSetting(ESettingCategory eCategory, string strKey)
        {
            this.eCategory = eCategory;
            this.strKey = strKey;
        }

        public string tEncode(bool value) => value.ToString();

        public bool tDecode(string strValue) => bool.Parse(strValue);
    }
}
