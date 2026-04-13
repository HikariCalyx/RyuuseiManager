using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace RyuuseiManager.API
{
    public class SteamInterop
    {
        const ulong STEAMID64_BASE = 76561197960265728;
        const int GAMEID = 3500390;

        public static ulong GetSteamID64(ulong steamID3)
        {
            return STEAMID64_BASE + steamID3;
        }

        public static ulong GetSteamID3(ulong steamID64)
        {
            return steamID64 - STEAMID64_BASE;
        }

        public static string? GetSteamPath()
        {
            switch (DB.GetSteamPathToggle())
            {
                default:
                case "0":
                    if (WineCheck.IsRunningUnderWine())
                    {
                        string userName = Environment.UserName;
                        string steamPath = $"Z:\\home\\{userName}\\.steam\\steam\\"; // Default case
                        if (!Directory.Exists(steamPath))
                        {
                            steamPath = $"Z:\\home\\{userName}\\.local\\share\\Steam\\"; // rare case
                        }
                        if (!Directory.Exists(steamPath))
                        {
                            steamPath = $"Z:\\home\\{userName}\\.local\\share\\SteamBeta\\"; // Beta client
                        }
                        if (!Directory.Exists(steamPath))
                        {
                            steamPath = $"Z:\\home\\{userName}\\.var\\app\\com.valvesoftware.Steam\\data\\Steam\\"; // Flatpak
                        }
                        if (!Directory.Exists(steamPath))
                        {
                            steamPath = $"Z:\\home\\{userName}\\snap\\steam\\common\\.local\\share\\Steam\\"; // Snap
                        }
                        return steamPath;
                    }
                    else
                    {
                        string? regPath = Registry.GetValue(
                        @"HKEY_CURRENT_USER\Software\Valve\Steam",
                        "SteamPath",
                        null
                        ) as string;
                        return regPath;
                    }
                case "1":
                    return DB.GetCustomSteamPath();
            }
        }

        public static string? GetLocalNickname(ulong steamID3, string? steamDir = null)
        {
            if (string.IsNullOrEmpty(steamDir))
            {
                steamDir = GetSteamPath() ?? "";
            }
            // Default Steam path (Windows)
            string localUserNameDir = Path.Combine(
                steamDir, "userdata", steamID3.ToString(), "config", "localconfig.vdf"
            );

            if (!File.Exists(localUserNameDir))
                return null;

            string text = File.ReadAllText(localUserNameDir);

            // Regex to find the nickname for this SteamID64
            string pattern = $"\"{steamID3}\"[\\s\\S]*?\"PersonaName\"\\s*\"([^\"]+)\"";
            var match = Regex.Match(text, pattern);

            if (!match.Success)
                return null;

            return match.Groups[1].Value;
        }

        public static string? GetSaveDataPath(ulong steamID3, string? steamDir = null)
        {
            if (string.IsNullOrEmpty(steamDir))
            {
                steamDir = GetSteamPath() ?? "";
            }
            return Path.GetFullPath(Path.Combine(steamDir, "userdata", steamID3.ToString(), GAMEID.ToString(), "remote", "win64_save")).TrimEnd(Path.DirectorySeparatorChar);
        }

        public static List<ulong> GetAvailableSteamUsers(string? steamDir = null)
        {
            List<ulong> users = new List<ulong>();
            if (string.IsNullOrEmpty(steamDir))
            {
                steamDir = GetSteamPath() ?? "";
            }
            if (Directory.Exists(steamDir))
            {
                string localUserDir = Path.Combine(steamDir, "userdata");
                if (!Directory.Exists(localUserDir)) Directory.CreateDirectory(localUserDir);
                var userids = Directory.GetDirectories(localUserDir);
                foreach (var i in userids)
                {
                    string steamID3 = Path.GetFileName(Path.GetFullPath(i).TrimEnd(Path.DirectorySeparatorChar));
                    if (Directory.Exists(Path.Combine(localUserDir, steamID3, GAMEID.ToString())))
                    {
                        if (ulong.TryParse(steamID3, out ulong id))
                        {
                            users.Add(id);
                        }
                    }
                }
            }
            return users;
        }
    }
}
