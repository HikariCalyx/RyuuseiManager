using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace RyuuseiManager
{
    class DB
    {
        private static readonly string dbFilePath = Path.Combine(
            AppContext.BaseDirectory,
            "savedata.db");
        private static readonly string connectionString = $"Data Source={dbFilePath};Version=3;";
        private static bool isInitialized = false;

        /// <summary>
        /// Initialize database and create tables if they don't exist.
        /// </summary>
        public static void InitDatabase()
        {
            if (isInitialized)
                return;

            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }

            var tableSqlCmds = new[]
            {
                @"CREATE TABLE IF NOT EXISTS saves (
                    save_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    savename TEXT NOT NULL,
                    saveblob BLOB NOT NULL,
                    generation INTEGER NOT NULL
                );",
                @"CREATE TABLE IF NOT EXISTS config (
                    variable TEXT NOT NULL,
                    value TEXT NOT NULL
                );",
                @"CREATE TABLE IF NOT EXISTS steamfamilyflag (
                    steamid3 INTEGER NOT NULL,
                    value INTEGER NOT NULL
                );"
            };

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    foreach (var sql in tableSqlCmds)
                    {
                        using (var cmd = new SQLiteCommand(sql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                isInitialized = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize database.", ex);
            }
        }

        /// <summary>
        /// Get config value by key, returns null if not found.
        /// </summary>
        private static string? GetConfigValue(string key)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        "SELECT value FROM config WHERE variable = @key LIMIT 1;", conn))
                    {
                        cmd.Parameters.AddWithValue("@key", key);
                        var result = cmd.ExecuteScalar();
                        return result as string;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get config value for key '{key}'.", ex);
            }
        }

        /// <summary>
        /// Set config value, inserting if not found or updating if exists.
        /// </summary>
        private static void SetConfigValue(string key, string value)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Check if key exists
                    string checkSql = "SELECT COUNT(*) FROM config WHERE variable = @key;";
                    using (var cmd = new SQLiteCommand(checkSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@key", key);
                        int count = (int)(long)cmd.ExecuteScalar();

                        string sql = count > 0
                            ? "UPDATE config SET value = @value WHERE variable = @key;"
                            : "INSERT INTO config (variable, value) VALUES (@key, @value);";

                        using (var updateCmd = new SQLiteCommand(sql, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@key", key);
                            updateCmd.Parameters.AddWithValue("@value", value);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to set config value for key '{key}'.", ex);
            }
        }

        public static string? GetCurrentLanguage()
        {
            InitDatabase();
            return GetConfigValue("lang");
        }

        public static void SetLanguage(string langCode)
        {
            InitDatabase();
            SetConfigValue("lang", langCode);
            App.SetLanguage(langCode);
        }

        public static string? GetCustomSteamPath()
        {
            InitDatabase();
            return GetConfigValue("steampath");
        }

        public static void SetSteamPath(string steampath)
        {
            InitDatabase();
            SetConfigValue("steampath", steampath);
        }

        public static string GetSteamPathToggle()
        {
            InitDatabase();
            var result = GetConfigValue("usecustompath");
            return string.IsNullOrEmpty(result) ? "unset" : result;
        }

        public static void ToggleCustomSteamPath(bool isEnabled)
        {
            InitDatabase();
            SetConfigValue("usecustompath", isEnabled ? "1" : "0");
        }

        public static int GetToggleSwitch(string switchName)
        {
            InitDatabase();
            var result = GetConfigValue(switchName);
            return string.IsNullOrEmpty(result) ? -1 : (result == "1" ? 1 : 0);
        }

        public static void SetToggleSwitch(string switchName, bool isEnabled)
        {
            InitDatabase();
            SetConfigValue(switchName, isEnabled ? "1" : "0");
        }

        /// <summary>
        /// Save data blob to database with optional compression.
        /// </summary>
        public static void SaveDataBlob(
            byte[] blob,
            string name,
            int generation,
            bool compression,
            out ulong saveId)
        {
            InitDatabase();
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        @"INSERT INTO saves (savename, saveblob, generation) 
                          VALUES (@name, @blob, @gen);", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@blob", compression ? CompressZlib(blob) : blob);
                        cmd.Parameters.AddWithValue("@gen", generation);
                        cmd.ExecuteNonQuery();
                        saveId = (ulong)conn.LastInsertRowId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save data blob.", ex);
            }
        }

        /// <summary>
        /// Replace save blob data for existing save ID.
        /// </summary>
        public static void ReplaceSaveBlob(byte[] blob, ulong saveId)
        {
            InitDatabase();
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        "UPDATE saves SET saveblob = @blob WHERE save_id = @id;", conn))
                    {
                        cmd.Parameters.AddWithValue("@blob", CompressZlib(blob));
                        cmd.Parameters.AddWithValue("@id", saveId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to replace save blob.", ex);
            }
        }

        /// <summary>
        /// Rename save by ID.
        /// </summary>
        public static void RenameSaveBlob(string newName, ulong saveId)
        {
            InitDatabase();
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        "UPDATE saves SET savename = @name WHERE save_id = @id;", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", newName);
                        cmd.Parameters.AddWithValue("@id", saveId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to rename save blob.", ex);
            }
        }

        /// <summary>
        /// Delete save by ID and vacuum database.
        /// </summary>
        public static void DeleteSaveById(ulong saveId)
        {
            InitDatabase();
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        "DELETE FROM saves WHERE save_id = @id;", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", saveId);
                        cmd.ExecuteNonQuery();
                    }
                    // Vacuum to reclaim space
                    using (var vacuumCmd = new SQLiteCommand("VACUUM;", conn))
                    {
                        vacuumCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete save.", ex);
            }
        }

        /// <summary>
        /// Load and decompress data blob by save ID. Returns null if not found.
        /// </summary>
        public static byte[]? LoadDataBlob(ulong saveId)
        {
            InitDatabase();
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        "SELECT saveblob FROM saves WHERE save_id = @id LIMIT 1;", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", saveId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return null;

                            byte[] compressed = (byte[])reader["saveblob"];
                            return DecompressZlib(compressed);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load data blob.", ex);
            }
        }

        /// <summary>
        /// Get all saves for a specific generation.
        /// </summary>
        public static Dictionary<int, string> GetCurrentGenerationSaves(int generation)
        {
            InitDatabase();
            var result = new Dictionary<int, string>();

            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        @"SELECT save_id, savename FROM saves WHERE generation = @gen;", conn))
                    {
                        cmd.Parameters.AddWithValue("@gen", generation);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                result[id] = name;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to get saves for generation {generation}.", ex);
            }

            return result;
        }

        /// <summary>
        /// Get save name by generation and save ID.
        /// </summary>
        public static string? GetSaveName(int generation, ulong saveId)
        {
            InitDatabase();
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(
                        @"SELECT savename FROM saves 
                          WHERE generation = @gen AND save_id = @id;", conn))
                    {
                        cmd.Parameters.AddWithValue("@gen", generation);
                        cmd.Parameters.AddWithValue("@id", saveId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to get save name for generation {generation}, save ID {saveId}.", ex);
            }

            return null;
        }

        /// <summary>
        /// Compress data using Zlib deflate compression.
        /// </summary>
        private static byte[] CompressZlib(byte[] data)
        {
            using (var output = new MemoryStream())
            {
                using (var deflate = new DeflateStream(output, CompressionLevel.Optimal, leaveOpen: true))
                {
                    deflate.Write(data, 0, data.Length);
                }
                return output.ToArray();
            }
        }

        /// <summary>
        /// Decompress data using Zlib deflate decompression.
        /// </summary>
        private static byte[] DecompressZlib(byte[] compressed)
        {
            using (var input = new MemoryStream(compressed))
            {
                using (var deflate = new DeflateStream(input, CompressionMode.Decompress))
                {
                    using (var output = new MemoryStream())
                    {
                        deflate.CopyTo(output);
                        return output.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Choose suitable language code based on current UI culture.
        /// </summary>
        public static string ChooseSuitableLangCode()
        {
            string code = CultureInfo.CurrentUICulture.Name;
            if (Lang.LangName.LangList.ContainsKey(code))
            {
                return code;
            }

            return code switch
            {
                "zh-SG" or "zh-Hans" => "zh-CN",
                "zh-HK" or "zh-MO" or "zh-Hant" => "zh-TW",
                _ => "en"
            };
        }
    }
}
