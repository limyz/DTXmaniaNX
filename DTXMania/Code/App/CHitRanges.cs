using System;

namespace DTXMania
{
    /// <summary>
    /// A set of hit ranges for each <see cref="EJudgement"/>.
    /// </summary>
    [Serializable]
    public class CHitRanges
    {
        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Perfect"/> range.
        /// </summary>
        public int nPerfectSize;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Great"/> range.
        /// </summary>
        public int nGreatSize;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Great"/> range.
        /// </summary>
        public int nGoodSize;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Poor"/> range.
        /// </summary>
        public int nPoorSize;

        public CHitRanges(int nDefaultSize = 0)
        {
            nPerfectSize = nGreatSize = nGoodSize = nPoorSize = nDefaultSize;
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
        public static CHitRanges tCompose(CHitRanges first, CHitRanges fallback) => new CHitRanges
        {
            nPerfectSize = (first.nPerfectSize >= 0) ? first.nPerfectSize : fallback.nPerfectSize,
            nGreatSize = (first.nGreatSize >= 0) ? first.nGreatSize : fallback.nGreatSize,
            nGoodSize = (first.nGoodSize >= 0) ? first.nGoodSize : fallback.nGoodSize,
            nPoorSize = (first.nPoorSize >= 0) ? first.nPoorSize : fallback.nPoorSize,
        };

        /// <summary>
        /// Copy all the values from the given <see cref="CHitRanges"/> into this set.
        /// </summary>
        /// <param name="other">The <see cref="CHitRanges"/> to copy from.</param>
        public void tCopyFrom(CHitRanges other)
        {
            nPerfectSize = other.nPerfectSize;
            nGreatSize = other.nGreatSize;
            nGoodSize = other.nGoodSize;
            nPoorSize = other.nPoorSize;
        }
    }
}
