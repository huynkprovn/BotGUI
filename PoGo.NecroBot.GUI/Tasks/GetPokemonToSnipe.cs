using Newtonsoft.Json;
using PoGo.NecroBot.GUI.Util;
using PoGo.NecroBot.GUI.Utils;
using PoGo.NecroBot.Logic.Logging;
using POGOProtos.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoGo.NecroBot.GUI.Tasks
{
    class GetPokemonToSnipe
    {
        public class DiscordWebReader
        {
            public static DiscordWebReader _discordWebReader;
            public static Stream stream;

            public DiscordWebReader()
            {
                InitializeWebClient();
            }

            //private WebClient Wc { get; set; }

            public void InitializeWebClient()
            {
                var request = WebRequest.Create(new Uri("http://pogo-feed.mmoex.com/messages"));
                ((HttpWebRequest)request).AllowReadStreamBuffering = false;

                try
                {
                    var response = request.GetResponse();
                    Logger.Write($"Connection established. Waiting for data...",LogLevel.Warning);
                    stream = response.GetResponseStream();
                }
                catch (WebException)
                {
                    Logger.Write($"Experiencing connection issues. Throttling...", LogLevel.Warning);
                    Task.Delay(30 * 1000);
                }
                catch (Exception e)
                {
                    Logger.Write($"Exception: {e}\n\n\n", LogLevel.Warning);
                }
            }

            public async static Task<Task> StartPollDiscordFeed(Stream stream)
            {
                return await Task.Factory.StartNew(async () => await DiscordThread(stream), TaskCreationOptions.LongRunning);
            }

            public async static Task DiscordThread(Stream stream)
            {
                const int delay = 10 * 1000;
                while (true)
                {
                    for (var retrys = 0; retrys <= 3; retrys++)
                    {
                        foreach (var line in ReadLines(new StreamReader(stream)))
                        {
                            try
                            {
                                var splitted = line.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);

                                if (splitted.Length != 2 || splitted[0] != "data") continue;

                                var jsonPayload = splitted[1];
                                //Log.Debug($"JSON: {jsonPayload}");

                                var result = JsonConvert.DeserializeObject<DiscordWebReader.DiscordMessage>(jsonPayload);

                                if (result == null) continue;

                                //Console.WriteLine($"Discord message received: {result.channel_id}: {result.content}");
                                //Logger.Write(String.Format("Discord message received: {0}: {1}", result.channel_id,
                                //    result.content),LogLevel.Warning);

                                MessageParser newMsg = new MessageParser();
                                var msgList = newMsg.parseMessage(result.content);
                                foreach(var msg in msgList)
                                {
                                    if(!Bot.PokemonSnipeFeed.Contains(msg))
                                    {
                                        int sort = Bot.GUI.DataGridSnipePokemons.Rows.Count + 1;
                                        Bot.GUI.DataGridSnipePokemons.Invoke(new Action(() => Bot.GUI.DataGridSnipePokemons.Rows.Add(msg.Id, msg.IV, msg.Latitude, msg.Longitude, msg.ExpirationTimestamp, sort)));
                                        Bot.GUI.DataGridSnipePokemons.Invoke(new Action(() => Bot.GUI.DataGridSnipePokemons.Sort(Bot.GUI.DataGridSnipePokemons.Columns["dataSnipingFeederColSort"],System.ComponentModel.ListSortDirection.Descending)));
                                        Bot.PokemonSnipeFeed.Add(msg);
                                        Logger.Write(msg.ToString(), LogLevel.Warning);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Write($"Exception:" + e);
                            }
                        }
                        Task.Delay(delay);
                    }
                }
            }

            private static IEnumerable<string> ReadLines(StreamReader stream)
            {
                var sb = new StringBuilder();

                var symbol = stream.Peek();
                while (symbol != -1)
                {
                    symbol = stream.Read();
                    sb.Append((char)symbol);

                    if (stream.Peek() != 10) continue;

                    stream.Read();
                    var line = sb.ToString();
                    sb.Clear();

                    yield return line;
                }

                yield return sb.ToString();
            }

            public async static Task<Task> TryStartDiscordReader()
            {
                while (true)
                {
                    _discordWebReader = new DiscordWebReader();

                    try
                    {
                        return await StartPollDiscordFeed(stream);
                    }
                    catch (WebException)
                    {
                        Logger.Write($"Experiencing connection issues. Throttling...",LogLevel.Warning);
                        Task.Delay(30 * 1000);
                        _discordWebReader.InitializeWebClient();
                    }
                    catch (Exception e)
                    {
                        Logger.Write($"Unknown exception "+e, LogLevel.Warning);
                        continue;
                    }
                    finally
                    {
                        Task.Delay(20 * 1000);
                    }
                }
            }

            public class AuthorStruct
            {
                public string avatar;
                public string discriminator;
                public string id;
                public string username;
            }

            public class DiscordMessage
            {
                public string channel_id = "";
                //public List<AuthorStruct> author;
                public string content = "";
                public string id = "";
                public string timestamp = "";
                //public string edited_timestamp = null;
                public bool tts = false;
                //public string mentions = "";
                //public string nonce = "";
                //public bool deleted = false;
                //public bool pinned = false;
                //public bool mention_everyone = false;
                //public string mention_roles = "";
                //public xxx attachments = "";
                //public xxx embeds = "";
            }
        }
    }

    public class SniperInfo
    {
        public ulong EncounterId { get; set; }
        public DateTime ExpirationTimestamp { get; set; } = default(DateTime);
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public PokemonId Id { get; set; } = PokemonId.Missingno;
        public string SpawnPointId { get; set; }
        public PokemonMove Move1 { get; set; }
        public PokemonMove Move2 { get; set; }
        public double IV { get; set; }

        public override string ToString()
        {
            return "SniperInfo: id: " + Id
                   + ", Latitude: " + Latitude
                   + ", Longitude: " + Longitude
                   + (IV != default(double) ? ", IV: " + IV + "%" : "")
                   + (ExpirationTimestamp != default(DateTime) ? ", expiration: " + ExpirationTimestamp : "");
        }
    }
}
