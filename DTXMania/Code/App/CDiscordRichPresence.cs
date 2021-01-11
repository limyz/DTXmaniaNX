using DiscordRPC;
using DiscordRPC.Message;
using System;
using System.Diagnostics;

namespace DTXMania
{
    /// <summary>
    /// An object which manages a single instance of <see href="https://discord.com/rich-presence">Discord Rich Presence</see> integration.
    /// </summary>
    /// <remarks>
    /// Note that only one instance is intended to be used at a time, multiple simultaneous instances leads to undefined behaviour.
    /// </remarks>
    public class CDiscordRichPresence : IDisposable
    {
        /// <summary>
        /// The unique identifier of the Discord Application used to present Rich Presence.
        /// </summary>
        private const string application_id = @"<CLIENT ID>";

        /// <summary>
        /// The client used by this instance.
        /// </summary>
        private DiscordRpcClient client;

        public CDiscordRichPresence()
        {
            client = new DiscordRpcClient(application_id);
            client.OnReady += (s, a) => Trace.TraceInformation($"Discord Rich Presence ready.");
            client.OnError += (s, a) => Trace.TraceError($"Discord Rich Presence error: {a.Message} ({a.Code})");

            // if this occurs then discord is likely not open,
            // so dont continue spamming connection attempts
            client.OnConnectionFailed += (s, a) =>
            {
                Trace.TraceInformation($"Unable to connect to Discord, disabling Rich Presence.");
                client.Deinitialize();
            };

            client.Initialize();
        }

        public void Dispose()
        {
            client?.Dispose();
            client = null;
        }

        /// <summary>
        /// Set this instance's presence to the given <see cref="RichPresence"/>.
        /// </summary>
        /// <param name="presence">The presence to set.</param>
        public void SetPresence(RichPresence presence) => client.SetPresence(presence);
    }
}
