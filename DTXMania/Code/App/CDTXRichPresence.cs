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
                LargeImageKey = CDTXMania.ConfigIni.strDiscordRichPresenceLargeImageKey,
                LargeImageText = CDTXMania.VERSION,
                SmallImageKey = CDTXMania.ConfigIni.bGuitarRevolutionMode ? CDTXMania.ConfigIni.strDiscordRichPresenceSmallImageKeyGuitar : CDTXMania.ConfigIni.strDiscordRichPresenceSmallImageKeyDrums,
                SmallImageText = CDTXMania.ConfigIni.bGuitarRevolutionMode ? "Guitar and Bass" : "Drums",
            };
        }
    }
}
