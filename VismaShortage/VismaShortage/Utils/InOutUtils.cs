using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using VismaShortage.Models;

namespace VismaShortage.Utils
{
    public class InOutUtils
    {
        const string pathToUsers = @"Data\users.json";
        const string pathToShortages = @"Data\shortages.json";

        public static List<User>? ReadUsers()
        {
            string usersJson = File.ReadAllText($"..\\..\\..\\{pathToUsers}");
            List<User>? users = JsonConvert.DeserializeObject<List<User>?>(usersJson);
            return users;
        }

        public static List<Shortage>? ReadShortages()
        {
            string shortagesJson = File.ReadAllText($"..\\..\\..\\{pathToShortages}");
            string correctedShortagesJson = Regex.Replace(shortagesJson, " room", "Room");
            List<Shortage>? shortages = JsonConvert.DeserializeObject<List<Shortage>?>(correctedShortagesJson, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter> 
                { 
                    new StringEnumConverter(),
                }
            });
            return shortages;
        }

        public static void PrintShortages(List<Shortage> shortages)
        {
            foreach (Shortage shortage in shortages)
            {
                Console.WriteLine($"Title: {shortage.Title}");
                Console.WriteLine($"Name: {shortage.Name}");
                string roomString = shortage.Room.ToString();
                string modifiedRoom = Regex.Replace(roomString, "Room", " room");
                Console.WriteLine($"Room: {modifiedRoom}");
                Console.WriteLine($"Category: {shortage.Category}");
                Console.WriteLine($"Priority: {shortage.Priority}");
                Console.WriteLine($"Created on: {shortage.CreatedOn}");
                Console.WriteLine();
            }
        }

        public static void WriteShortages(List<Shortage> shortages)
        {
            string shortagesJson = JsonConvert.SerializeObject(shortages, new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                },
                Formatting = Formatting.Indented
            })!;
            string correctedShortagesJson = Regex.Replace(shortagesJson, @"(?<=\w)Room", " room");
            File.WriteAllText($"..\\..\\..\\{pathToShortages}", correctedShortagesJson);
        }
    }
}
