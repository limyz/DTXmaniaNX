namespace DTXMania
{
    /// <summary>
    /// A <see cref="bool"/>-backed <see cref="ISetting{T}"/>.
    /// </summary>
    public class CBoolSetting : ISetting<bool>
    {
        public string strKey { get; }

        public CBoolSetting(string strKey)
        {
            this.strKey = strKey;
        }

        public string tEncode(bool value) => value.ToString();

        public bool tDecode(string strValue) => bool.Parse(strValue);
    }
}
