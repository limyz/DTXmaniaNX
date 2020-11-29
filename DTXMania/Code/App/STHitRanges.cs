using System;

namespace DTXMania
{
    /// <summary>
    /// A set of hit ranges for each <see cref="EJudgement"/>.
    /// </summary>
    [Serializable]
    public struct STHitRanges
    {
        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Perfect"/> range.
        /// </summary>
        public int nPerfectSizeMs;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Great"/> range.
        /// </summary>
        public int nGreatSizeMs;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Good"/> range.
        /// </summary>
        public int nGoodSizeMs;

        /// <summary>
        /// The size, in ± milliseconds, of the <see cref="EJudgement.Poor"/> range.
        /// </summary>
        public int nPoorSizeMs;

        public STHitRanges(int nDefaultSizeMs = 0)
        {
            nPerfectSizeMs = nGreatSizeMs = nGoodSizeMs = nPoorSizeMs = nDefaultSizeMs;
        }

        /// <summary>
        /// Create a new <see cref="STHitRanges"/> with the default DTXMania judgement ranges.
        /// </summary>
        /// <remarks>
        /// This exists as a legacy method to obtain these values in usages where they served as defaults.
        /// </remarks>
        /// <returns>A default DTXMania <see cref="STHitRanges"/>.</returns>
        public static STHitRanges tCreateDTXHitRanges() => new STHitRanges
        {
            nPerfectSizeMs = 34,
            nGreatSizeMs = 67,
            nGoodSizeMs = 84,
            nPoorSizeMs = 117,
        };

        /// <summary>
        /// Compose and return a new <see cref="STHitRanges"/> from the values of the two given sets.
        /// </summary>
        /// <remarks>
        /// A value within a set is considererd set when it is greater than or equal to zero. <br/>
        /// It is assumed that <paramref name="fallback"/> has each value set.
        /// </remarks>
        /// <param name="first">The set that should be checked first for a value.</param>
        /// <param name="fallback">The set containing values to fall back to if the first set does not have one.</param>
        /// <returns>The new <see cref="STHitRanges"/> composed of the two given sets.</returns>
        public static STHitRanges tCompose(STHitRanges first, STHitRanges fallback) => new STHitRanges
        {
            nPerfectSizeMs = (first.nPerfectSizeMs >= 0) ? first.nPerfectSizeMs : fallback.nPerfectSizeMs,
            nGreatSizeMs = (first.nGreatSizeMs >= 0) ? first.nGreatSizeMs : fallback.nGreatSizeMs,
            nGoodSizeMs = (first.nGoodSizeMs >= 0) ? first.nGoodSizeMs : fallback.nGoodSizeMs,
            nPoorSizeMs = (first.nPoorSizeMs >= 0) ? first.nPoorSizeMs : fallback.nPoorSizeMs,
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
                case var t when t <= nPerfectSizeMs:
                    return EJudgement.Perfect;
                case var t when t <= nGreatSizeMs:
                    return EJudgement.Great;
                case var t when t <= nGoodSizeMs:
                    return EJudgement.Good;
                case var t when t <= nPoorSizeMs:
                    return EJudgement.Poor;
                default:
                    return EJudgement.Miss;
            }
        }
    }
}
