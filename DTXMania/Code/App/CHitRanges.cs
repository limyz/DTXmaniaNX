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
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Good"/> range.
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
        /// Create a new <see cref="CHitRanges"/> with the default DTXMania judgement ranges.
        /// </summary>
        /// <remarks>
        /// This exists as a legacy method to obtain these values in usages where they served as defaults.
        /// </remarks>
        /// <returns>A default DTXMania <see cref="CHitRanges"/>.</returns>
        public static CHitRanges tCreateDTXHitRanges() => new CHitRanges
        {
            nPerfectSize = 34,
            nGreatSize = 67,
            nGoodSize = 84,
            nPoorSize = 117,
        };

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
        /// Get the <see cref="EJudgement"/> which would occur from hitting a chip at the given absolute offset from its playback time, when using this set.
        /// </summary>
        /// <param name="nDeltaTimeMs">The absolute offset, in milliseconds, from the <see cref="CDTX.CChip.nPlaybackTimeMs"/> of the chip.</param>
        /// <returns>The <see cref="EJudgement"/> for <paramref name="nDeltaTimeMs"/>.</returns>
        public EJudgement tGetJudgement(int nDeltaTimeMs)
        {
            switch (nDeltaTimeMs)
            {
                case var t when t <= nPerfectSize:
                    return EJudgement.Perfect;
                case var t when t <= nGreatSize:
                    return EJudgement.Great;
                case var t when t <= nGoodSize:
                    return EJudgement.Good;
                case var t when t <= nPoorSize:
                    return EJudgement.Poor;
                default:
                    return EJudgement.Miss;
            }
        }
    }
}
