using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using  Zepheus.Util;
using Zepheus.Database;
using Zepheus.Database.Storage;
using System.Data;

namespace Zepheus.Database.DataStore
{
    public class ReadMethods
    {
        private const string SelectQuickBar = "SELECT QuickBar FROM characters WHERE CharID=@characterID";
        private const string SelectShortcuts = "SELECT Shortcuts FROM characters WHERE CharID=@characterID";
        private const string SelectQuickBarState = "SELECT QuickBarState FROM characters WHERE CharID=@characterID";
        private const string SelectClientSettings = "SELECT ClientSettings FROM characters WHERE CharID=@characterID";
        private const string SelectGameSettings = "SELECT ClientSettings FROM characters WHERE CharID=@characterID";
        #region Blobs

        public static byte[] GetQuickBar(int CharID,DatabaseManager DbManager)
        {
            try
            {
                var command = new MySqlCommand(SelectQuickBar);
              byte[] Quickbar;
                using (DatabaseClient dbClient = DbManager.GetClient())
                {
                    command.Connection = dbClient.Connection;
                    command.Parameters.AddWithValue("@characterID", CharID);
                     Quickbar = dbClient.GetBlob(command);
                }
               //byte[] Quickbar = DbManager.GetClient().GetBlob(command);
                return Quickbar;
            }
            catch
            {
                Log.WriteLine(LogLevel.Error, "GetQuickbar Failed from datatbase");
                return null;
            }
        }
        public static byte[] GetShortcuts(int CharID, DatabaseManager DbManager)
        {
            try
            {
                var command = new MySqlCommand(SelectShortcuts);
                byte[] Shortcuts;
                using (DatabaseClient dbClient = DbManager.GetClient())
                {
                    command.Connection = dbClient.Connection;
                    command.Parameters.AddWithValue("@characterID", CharID);
                    Shortcuts = dbClient.GetBlob(command);
                }

            
                return Shortcuts;
            }
            catch
            {
                Log.WriteLine(LogLevel.Error, "GetShortcuts Failed from datatbase");
                return null;
            }
        }
        public static byte[] GetQuickBarState(int CharID, DatabaseManager DbManager)
        {
            try
            {
                var command = new MySqlCommand(SelectQuickBarState);
                byte[] QuickBarState;
                using (DatabaseClient dbClient = DbManager.GetClient())
                {
                    command.Connection = dbClient.Connection;
                    command.Parameters.AddWithValue("@characterID", CharID);
                    QuickBarState = dbClient.GetBlob(command);
                }
               
                return QuickBarState;
            }
            catch
            {
                Log.WriteLine(LogLevel.Error, "GetQuickBarState Failed from datatbase");
                return null;
            }
        }

        public static byte[] GetGameSettings(int CharID, DatabaseManager DbManager)
        {
            try
            {
                var command = new MySqlCommand(SelectGameSettings);
                byte[] GameSettings;
                using (DatabaseClient dbClient = DbManager.GetClient())
                {
                    command.Connection = dbClient.Connection;
                    command.Parameters.AddWithValue("@characterID", CharID);
                    GameSettings = dbClient.GetBlob(command);
                }
                return GameSettings;
            }
            catch
            {
                Log.WriteLine(LogLevel.Error, "GetGameSettings Failed from datatbase");
                return null;
            }
        }
        public static byte[] GetClientSettings(int CharID, DatabaseManager DbManager)
        {
            try
            {
                var command = new MySqlCommand(SelectClientSettings);
                byte[] ClientSetting;
                using (DatabaseClient dbClient = DbManager.GetClient())
                {
                    command.Connection = dbClient.Connection;
                    command.Parameters.AddWithValue("@characterID", CharID);
                    ClientSetting = dbClient.GetBlob(command);
                }
                return ClientSetting;
            }
            catch
            {
                Log.WriteLine(LogLevel.Error, "GetClientSettings Failed from datatbase");
                return null;
            }
        }
        #endregion
        #region Converts
        public static bool EnumToBool(string Enum)
        {
            if (Enum == "1")
            {
                return true;
            }

            return false;
        }
        #endregion
        public static Character ReadCharObjectFromDatabase(string Charname,DatabaseManager Dbmanager)
        {
            Character ch = new Character();
                DataTable CharData = null;
                 using (DatabaseClient dbClient = Dbmanager.GetClient())
            {
                CharData = dbClient.ReadDataTable("SELECT  *FROM `characters` WHERE binary `Name` = '" + Charname + "'");
            }
                 if (CharData != null)
                 {
                     foreach (DataRow Row in CharData.Rows)
                     {
                         ch.PositionInfo.ReadFromDatabase(Row);
                         ch.AccountID = int.Parse(Row["AccountID"].ToString());
                         ch.LookInfo.ReadFromDatabase(Row);
                         ch.CharacterStats.ReadFromDatabase(Row);
                         ch.Slot = (byte)Row["Slot"];
                         ch.CharLevel = (byte)Row["Level"];
                         ch.Name = (string)Row["Name"];
                         ch.ID = int.Parse(Row["CharID"].ToString());
                         ch.Job = (byte)Row["Job"];
                         ch.Money = long.Parse(Row["Money"].ToString());
                         ch.Exp = long.Parse(Row["Exp"].ToString());
                         ch.HP = int.Parse(Row["CurHP"].ToString()); ;
                         ch.HPStones = 10;
                         ch.SP = int.Parse(Row["CurSP"].ToString()); ;
                         ch.SPStones = 10;
                         ch.StatPoints = (byte)Row["StatPoints"];
                         ch.UsablePoints = (byte)Row["UsablePoints"];
                         ch.Fame = 0;
                         ch.GameSettings = DataStore.ReadMethods.GetGameSettings(ch.ID, Dbmanager);
                         ch.ClientSettings = DataStore.ReadMethods.GetClientSettings(ch.ID, Dbmanager);
                         ch.Shortcuts = DataStore.ReadMethods.GetShortcuts(ch.ID, Dbmanager);
                         ch.QuickBar = DataStore.ReadMethods.GetQuickBar(ch.ID, Dbmanager);
                         ch.QuickBarState = DataStore.ReadMethods.GetQuickBarState(ch.ID, Dbmanager);
                     }
                 }
        
            try
            {
              
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return ch;
        }
    }

}