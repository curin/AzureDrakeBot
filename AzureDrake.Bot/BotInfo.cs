using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AzureDrake.Bot
{
    [Serializable]
    public struct BotInfo
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string ChannelName { get; set; }
        public string BotUsername { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string OAuth { get; set; }
        public string ConnectionString { get; set; }
        public int DefaultRank { get; set; }
        public int DefaultVIPRank { get; set; }
        public int DefaultModRank { get; set; }

        public static BotInfo Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BotInfo));

            if (File.Exists("BotInfo.xml"))
            {
                TextReader reader = new StreamReader("BotInfo.xml");
                return (BotInfo)serializer.Deserialize(reader);
            }
            else
            {
                TextWriter writer = new StreamWriter("BotInfo.xml", false);
                serializer.Serialize(writer, Default());
                writer.Close();
                return Default();
            }
        }

        public static BotInfo Default()
        {
            return new BotInfo()
            {
                ClientID = "Null",
                ClientSecret = "Null",
                ChannelName = "Null",
                BotUsername = "Null",
                AccessToken = "Null",
                RefreshToken = "Null",
                OAuth = "Null"
            };
        }
    }
}
