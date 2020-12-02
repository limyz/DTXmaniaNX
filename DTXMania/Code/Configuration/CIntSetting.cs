namespace DTXMania
{
    /// <summary>
    /// An <see cref="int"/>-backed <see cref="ISetting{T}"/>.
    /// </summary>
    public class CIntSetting : ISetting<int>
    {
        public string strKey { get; }

        public CIntSetting(string strKey)
        {
            this.strKey = strKey;
        }

        public string tEncode(int value) => value.ToString();

        public int tDecode(string strValue) => int.Parse(strValue);
    }
}
