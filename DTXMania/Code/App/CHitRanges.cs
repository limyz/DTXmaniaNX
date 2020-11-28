namespace DTXMania
{
    /// <summary>
    /// A set of hit ranges for each <see cref="EJudgement"/>.
    /// </summary>
    public class CHitRanges
    {
        /// <summary>
        /// The unique identifier of this set.
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

        public CHitRanges(string name, int defaultRange = 0)
        {
            Name = name;
            Perfect = Great = Good = Poor = defaultRange;
        }

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
