namespace DTXMania.Configuration.Settings
{
    /// <summary>
    /// An <see cref="int"/>-backed <see cref="ISetting{T}"/>.
    /// </summary>
    public class CIntSetting : ISetting<int>
    {
        /// <summary>
        /// The inclusive minimum value that this setting's value can be, or <see cref="null"/> if their is no lower limit.
        /// </summary>
        private readonly int? nMinValue;

        /// <summary>
        /// The inclusive maximum value that this setting's value can be, or <see cref="null"/> if their is no upper limit.
        /// </summary>
        private readonly int? nMaxValue;

        public ESettingCategory eCategory { get; }

        public string strKey { get; }

        public CIntSetting(ESettingCategory eCategory, string strKey, int? nMinValue = null, int? nMaxValue = null)
        {
            this.eCategory = eCategory;
            this.strKey = strKey;
            this.nMinValue = nMinValue;
            this.nMaxValue = nMaxValue;
        }

        public string tEncode(int value) => tLimitValueRange(value).ToString();

        public int tDecode(string strValue) => tLimitValueRange(int.Parse(strValue));

        /// <summary>
        /// Limit the given value to this setting's range.
        /// </summary>
        /// <param name="nValue">The <see cref="int"/> to limit.</param>
        private int tLimitValueRange(int nValue)
        {
            if (nMinValue is int nMin && nValue < nMin)
                return nMin;

            if (nMaxValue is int nMax && nValue > nMax)
                return nMax;

            return nValue;
        }
    }
}
