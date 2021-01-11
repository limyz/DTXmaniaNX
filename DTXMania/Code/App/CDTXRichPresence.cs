using DiscordRPC;

namespace DTXMania
{
    /// <summary>
    /// A <see cref="RichPresence"/> which populates with shared defaults.
    /// </summary>
    public class CDTXRichPresence : RichPresence
    {
        public CDTXRichPresence()
        {
            Assets = new Assets()
            {
                LargeImageKey = @"placeholder",
                LargeImageText = CDTXMania.VERSION,
                SmallImageKey = @"placeholder",
                SmallImageText = CDTXMania.ConfigIni.bGuitarRevolutionMode ? "Guitar and Bass" : "Drums",
            };
        }
    }
}
