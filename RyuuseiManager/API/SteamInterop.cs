using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            string? regPath = Registry.GetValue(
            @"HKEY_CURRENT_USER\Software\Valve\Steam",
            "SteamPath",
            null
            ) as string;
            return regPath;
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
            string pattern = $"\"{steamID3}\"[\\s\\S]*?\"name\"\\s*\"([^\"]+)\"";
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
