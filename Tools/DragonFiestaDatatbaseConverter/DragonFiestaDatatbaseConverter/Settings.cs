using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace DragonFiestaDatatbaseConverter
{
   public  class Settings
    {
        private const string ConfigName = "\\Config.cfg";
        private static readonly string ConfigPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + ConfigName;

        private static readonly Settings Instance = new Settings();
        public static string Comments { get { return _comments; } }
        private static string _comments = string.Empty;
        // TS: This is otherwise known as a dictionary:
        //private static List<KeyValuePair<object, object>> Properties;
        private readonly Dictionary<string, object> properties;
        private bool isInitialized;
        public static Random Random { get; private set; }

        private Settings()
        {
            this.properties = new Dictionary<string, object>();
            this.isInitialized = false;
        }

        /// <summary>
        /// Automatically loads settings from config file
        /// </summary>
        
        public static bool Initialize()
        {
            return Instance.InitializeInternal();
        }

        private bool InitializeInternal()
        {
            if (this.isInitialized) return true;
            try
            {
                this.ParseFile(ConfigPath);
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "Error reading settings: {0}", ex.Message);
                return false;
            }
            Random = new Random(DateTime.Now.Second);
            Log.WriteLine(LogLevel.Info, "Settings loaded successfully.");
            this.isInitialized = true;
            return true;
        }

        #region Get methods

        /// <summary>
        /// Gets a Boolean from the file
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>true if value is true, else returns false</returns>
        public static bool GetBool(string key)
        {
            return GetString(key).ToLower() == "true";
        }

        /// <summary>
        /// Gets an Int32 type variable from the file
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        /// <returns>'key's Int32 value</returns>
        public static int GetInt32(string key)
        {
            return Convert.ToInt32(Instance.properties[key]);
        }

        /// <summary>
        /// Gets an Int16 type variable from the file
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        /// <returns>'key's Int16 value</returns>
        public static short GetInt16(string key)
        {
            return Convert.ToInt16(Instance.properties[key]);
        }

        /// <summary>
        /// Gets an Byte type variable from the file
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        /// <returns>'key's Byte value</returns>
        public static byte GetByte(string key)
        {
            return Convert.ToByte(Instance.properties[key]);
        }

        /// <summary>
        /// Gets a String type variable from the file
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        /// <returns>'key's String vaule</returns>
        public static string GetString(string key)
        {
            return Instance.properties[key].ToString();
        }

        #endregion

        /// <summary>
        /// Reads the file and parse it into a List of Key Vaule Pairs.
        /// </summary>
        /// <param name="fileName">filepath</param>
        /// <returns>List of Key Value Pairs</returns>
        private void ParseFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            foreach (string entry in lines.Select(line => line.Trim()).Where(line => line.Length > 0))
            {
                if (!entry.Contains("#"))
                {
                    string[] parts = entry.Split('=');
                    if (parts.Length != 2) continue;

                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    if (!properties.ContainsKey(key))
                    {
                        properties.Add(key, value);
                    }
                }
                else
                {
                    _comments += Environment.NewLine + entry.Remove(0, 1);
                }
            }
        }
    }
}