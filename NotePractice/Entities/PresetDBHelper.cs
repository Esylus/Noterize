using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotePractice.Entities
{
    class PresetDBHelper
    {
        private static SQLiteConnection m_dbConnection;

        public static void CreateNewDatabase()
        {
            SQLiteConnection.CreateFile("NoterizePresets.sqlite");
        }

        public static void ConnectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=NoterizePresets.sqlite;Version=3;");
            m_dbConnection.Open();
        }

        public static void CreateTable()
        {
            string sql = "create table if not exists userPresets (name varchar(20), list varchar(300))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public static void InsertDefault()
        {
            string sql = "INSERT INTO 'userPresets' ('name', 'list') VALUES ('TrebleClef', '0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "INSERT INTO 'userPresets' ('name', 'list') VALUES ('BassClef', '23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public static void InsertPreset(Preset preset)
        {
            string sql = "INSERT INTO 'userPresets' ('name', 'list') VALUES ('" + preset.Name + "', '" + preset.UserList + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public static void PrintPresetsComboBox(ComboBox comboBox)
        {
            string sql = "select * from userPresets";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                comboBox.Items.Add(reader[0]);
            }

            m_dbConnection.Close();
        }

        public static string GetPresetList(string presetName)
        {

            string sql = "select * from userPresets where name = '" + presetName + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            string preset = "";

            while (reader.Read())
            {
                preset = (reader[1].ToString());
            }

            m_dbConnection.Close();

            return preset;

        }

    }
}
