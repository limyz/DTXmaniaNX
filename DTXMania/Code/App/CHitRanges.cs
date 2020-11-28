namespace DTXMania
{
    /// <summary>
    /// A set of hit ranges for each <see cref="EJudgement"/>.
    /// </summary>
    public class CHitRanges
    {
        /// <summary>
        /// The unique identifier of this set, or <see cref="null"/> if there is none.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Perfect"/> range.
        /// </summary>
        public int Perfect;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Great"/> range.
        /// </summary>
        public int Great;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Great"/> range.
        /// </summary>
        public int Good;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Poor"/> range.
        /// </summary>
        public int Poor;

        public CHitRanges(string name = null, int defaultRange = 0)
        {
            Name = name;
            Perfect = Great = Good = Poor = defaultRange;
        }

        /// <summary>
        /// Compose and return a new <see cref="CHitRanges"/> from the values of the two given sets.
        /// </summary>
        /// <remarks>
        /// A value within a set is considererd set when it is greater than or equal to zero. <br/>
        /// It is assumed that <paramref name="fallback"/> has each value set.
        /// </remarks>
        /// <param name="first">The set that should be checked first for a value.</param>
        /// <param name="fallback">The set containing values to fall back to if the first set does not have one.</param>
        /// <returns>The new <see cref="CHitRanges"/> composed of the two given sets.</returns>
        public static CHitRanges Compose(CHitRanges first, CHitRanges fallback) => new CHitRanges
        {
            Perfect = (first.Perfect >= 0) ? first.Perfect : fallback.Perfect,
            Great = (first.Great >= 0) ? first.Great : fallback.Great,
            Good = (first.Good >= 0) ? first.Good : fallback.Good,
            Poor = (first.Poor >= 0) ? first.Poor : fallback.Poor,
        };

        /// <summary>
        /// Copy all the values from the given <see cref="CHitRanges"/> into this set.
        /// </summary>
        /// <param name="other">The <see cref="CHitRanges"/> to copy from.</param>
        public void CopyFrom(CHitRanges other)
        {
            Perfect = other.Perfect;
            Great = other.Great;
            Good = other.Good;
            Poor = other.Poor;
        }
    }
}
