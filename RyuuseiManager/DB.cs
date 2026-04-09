using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace RyuuseiManager
{
    class DB
    {
        private static readonly string dbFilePath = Path.Combine(AppContext.BaseDirectory, "savedata.db");
        private static string connectionString = $"Data Source={dbFilePath};Version=3;";

        public static void InitDatabase()
        {
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
            string tableSqlCmd = "CREATE TABLE IF NOT EXISTS saves (" +
                "save_id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "savename TEXT NOT NULL," +
                "saveblob BLOB NOT NULL," +
                "generation INTEGER NOT NULL" +
                ");";
            string table2SqlCmd = "CREATE TABLE IF NOT EXISTS config (" +
                "variable TEXT NOT NULL," +
                "value TEXT NOT NULL" +
                ");";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(tableSqlCmd, conn);
                cmd.ExecuteNonQuery();
                cmd.CommandText = table2SqlCmd;
                cmd.ExecuteNonQuery();
            }
        }

        public static string GetCurrentLanguage()
        {
            InitDatabase();
            string sqlCommand = @"SELECT value FROM config WHERE variable = 'lang' LIMIT 1;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sqlCommand, conn);
                return (string)cmd.ExecuteScalar();
            }
        }

        public static void SetLanguage(string langCode)
        {
            InitDatabase();
            string sqlCommand = "";
            if (string.IsNullOrEmpty(GetCurrentLanguage()))
            {
                sqlCommand = @"INSERT INTO config (variable, value) VALUES ('lang', @name);";
            }
            else
            {
                sqlCommand = @"UPDATE config SET value = @name WHERE variable = 'lang';";
            }
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@name", langCode);
                cmd.ExecuteNonQuery();
            }
            App.SetLanguage(langCode);
        }

        public static void SaveDataBlob(byte[] blob, string name, int generation, bool compression, out ulong saveId)
        {
            InitDatabase();
            string sqlCommand = @"INSERT INTO saves (savename, saveblob, generation) VALUES (@name, @blob, @gen);";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@blob", compression ? CompressZlib(blob) : blob);
                cmd.Parameters.AddWithValue("@gen", generation);
                cmd.ExecuteNonQuery();
                saveId = (ulong)conn.LastInsertRowId;
            }
        }

        public static void RenameSaveBlob(string newName, ulong saveId)
        {
            string sqlCommand = @"UPDATE saves SET savename = @name WHERE save_id = @id;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@name", newName);
                cmd.Parameters.AddWithValue("@id", saveId);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteSaveById(ulong saveId)
        {
            InitDatabase();
            string sqlCommand = @"DELETE FROM saves WHERE save_id = @id;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@id", saveId);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "VACUUM;";
                cmd.ExecuteNonQuery();
            }
        }


        public static byte[]? LoadDataBlob(ulong saveId)
        {
            InitDatabase();
            string sqlCommand = @"SELECT saveblob FROM saves WHERE save_id = @id LIMIT 1;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sqlCommand, conn);
                cmd.Parameters.AddWithValue("@id", saveId);
                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return null;

                byte[] compressed = (byte[])reader["saveblob"];
                return DecompressZlib(compressed);
            }
        }

        public static Dictionary<int, string> GetCurrentGenerationSaves(int generation)
        {
            InitDatabase();
            var result = new Dictionary<int, string>();
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT save_id, savename 
                       FROM saves 
                       WHERE generation = @gen;";

                using (var cmd = new SQLiteCommand(sql, conn))
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

            return result;
        }

        private static byte[] CompressZlib(byte[] data)
        {
            using var output = new MemoryStream();
            using (var deflate = new DeflateStream(output, CompressionLevel.Optimal, leaveOpen: true))
            {
                deflate.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        private static byte[] DecompressZlib(byte[] compressed)
        {
            using var input = new MemoryStream(compressed);
            using var deflate = new DeflateStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();

            deflate.CopyTo(output);
            return output.ToArray();
        }

        public static string ChooseSuitableLangCode()
        {
            string code = CultureInfo.CurrentUICulture.Name;
            if (Lang.LangName.LangList.ContainsKey(code))
            {
                return code;
            }
            else
            {
                switch (code)
                {
                    case "zh-SG":
                    case "zh-Hans":
                        return "zh-CN";
                    case "zh-HK":
                    case "zh-MO":
                    case "zh-Hant":
                        return "zh-TW";
                    default:
                        return "en";
                }
            }
        }
    }
}
