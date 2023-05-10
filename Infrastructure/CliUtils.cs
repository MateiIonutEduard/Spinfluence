using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#pragma warning disable

namespace Spinfluence.Infrastructure
{
    public class CliUtils
    {
        private static bool IsPowerTwo(int val)
        {
            int logt = (int)Math.Log2(val);
            return ((1 << logt) == val);
        }

        private static byte[] GetBytes(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => (x & 1) == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static void ExecuteCommand(string[] args)
        {
            string content = File.ReadAllText("./Infrastructure/clisettings.json");
            List<CliCommand>? commands = JsonConvert.DeserializeObject<List<CliCommand>>(content);
            StringBuilder sb = new StringBuilder();

            string? first = args.FirstOrDefault();
            CliCommand cliCommand = commands
                .FirstOrDefault(c => c.CommandName.Contains(first));

            if(cliCommand.CommandValue != null && !IsPowerTwo(cliCommand.CommandValue.Value))
            {
                Console.WriteLine("Required number of bits to be power of two.");
                return;
            }

            if(first.Contains("--gen"))
            {
                if(first.CompareTo("--gen-bearer-sign-key") == 0)
                {
                    // fill bearer signing key
                    int bytes = cliCommand.CommandValue.Value >> 3;
                    byte[] bearerTokenSignKey = new byte[bytes];
                    RandomNumberGenerator.Fill(bearerTokenSignKey);

                    for (int i = 0; i < bytes; i++)
                        sb.Append(bearerTokenSignKey[i].ToString("X2"));

                    string data = File.ReadAllText("./appsettings.json");
                    dynamic obj = JsonConvert.DeserializeObject(data);

                    // update Bearer token security key
                    obj["JwtSettings"]["Key"] = Convert.ToBase64String(bearerTokenSignKey);
                    string newData = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented);

                    // save at disk
                    File.WriteAllText("./appsettings.json", newData);
                    Console.WriteLine($"Successful created Bearer Sign Key {sb.ToString()}");
                }
                else
                {
                    // fill secret sign key and salt protection key
                    int bytes = cliCommand.CommandValue.Value >> 3;
                    byte[] secretSignKey = new byte[bytes];

                    byte[] saltSignKey = new byte[bytes >> 1];
                    RandomNumberGenerator.Fill(secretSignKey);
                    RandomNumberGenerator.Fill(saltSignKey);

                    for (int i = 0; i < bytes; i++)
                        sb.Append(secretSignKey[i].ToString("X2"));

                    string data = File.ReadAllText("./appsettings.json");
                    dynamic obj = JsonConvert.DeserializeObject(data);

                    obj["AppSettings"]["secret"] = Convert.ToBase64String(secretSignKey);
                    obj["AppSettings"]["salt"] = Convert.ToBase64String(saltSignKey);

                    // update key to the app settings
                    string newData = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented);
                    File.WriteAllText("./appsettings.json", newData);

                    Console.WriteLine($"Successful created Secret Sign Key {sb.ToString()}");
                    sb.Clear();

                    for (int i = 0; i < (bytes >> 1); i++)
                        sb.Append(saltSignKey[i].ToString("X2"));

                    Console.WriteLine($"Successful created Protection Sign Key {sb.ToString()}");
                    sb.Clear();
                }
            }
            else
            {
                if (first.CompareTo("--bearer-sign-key") == 0)
                {
                    string next = args.Skip(1).Take(1).First();
                    string bearerTokenSigningKey = Convert.ToBase64String(GetBytes(next));

                    string data = File.ReadAllText("./appsettings.json");
                    dynamic obj = JsonConvert.DeserializeObject(data);

                    // update Bearer token security key
                    obj["JwtSettings"]["Key"] = bearerTokenSigningKey;
                    string newData = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented);
                    
                    File.WriteAllText("./appsettings.json", newData);
                    Console.WriteLine("Bearer token signing key was update successfully!");
                }
                else
                {
                    string secret = args.Skip(1).Take(1).First();
                    string salt = args.Skip(2).Take(1).First();

                    // read keys data in base64 format
                    string SecretSignKey = Convert.ToBase64String(GetBytes(secret));
                    string SaltSignKey = Convert.ToBase64String(GetBytes(salt));

                    string data = File.ReadAllText("./appsettings.json");
                    dynamic obj = JsonConvert.DeserializeObject(data);

                    obj["AppSettings"]["secret"] = SecretSignKey;
                    obj["AppSettings"]["salt"] = SaltSignKey;

                    string newData = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented);
                    File.WriteAllText("./appsettings.json", newData);
                    Console.WriteLine("AppSettings signing keys was update successfully!");
                }
            }
        }
    }
}
