/*    This file is part of Zepheus Emulator

    Zepheus Emulator is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Zepheus Emulator is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Zepheus Emulator.  If not, see <http://www.gnu.org/licenses/>.  */

using System;
using System.Threading;
using System.Text;
using System.IO;

namespace SHNtoMySQLConverter
{
    public static class Log
    {
        private static readonly Mutex Locker = new Mutex();
        public static bool IsDebug { get; set; }
        public static TextWriter Writer { get; set; }
        public static short FlushCount { get; set; }
        public static void WriteLine(LogLevel pLogLevel, string pFormat, params object[] pArgs)
        {
            if (pLogLevel == LogLevel.Debug && !IsDebug) return;
            string header = "[" + DateTime.Now + "] (" + pLogLevel + ") ";
            string buffer = string.Format(pFormat, pArgs);

            Locker.WaitOne();
            try
            {
                Console.ForegroundColor = GetColor(pLogLevel);
                Console.Write(header);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(buffer);
                if (Writer != null)
                {
                    Writer.WriteLine(header + buffer);
                    ++FlushCount;
                    if (FlushCount == 20)
                    {
                        Writer.Flush();
                        FlushCount = 0;
                    }
                    else if (!(pLogLevel == LogLevel.Info || pLogLevel == LogLevel.Debug))
                    {
                        Flush();
                    }
                }
            }
            finally
            {
                Locker.ReleaseMutex();
            }
            //TODO: txt files
        }
        public static void SetLogToFile(string filename)
        {
            Directory.CreateDirectory(filename.Replace(Path.GetFileName(filename), ""));
            StreamWriter sw = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read));
            sw.AutoFlush = true;
            Writer = sw;
        }
        public static void Flush()
        {
            if (Writer != null)
            {
                Writer.Flush();
                FlushCount = 0;
            }
        }

        public static void Dump(byte[] pBuffer, int pStart, int pLength)
        {
            if (pLength <= 0) return;

            string[] split = BitConverter.ToString(pBuffer, pStart, pLength).Split('-');
            StringBuilder hex = new StringBuilder(48);
            StringBuilder ascii = new StringBuilder(16);
            StringBuilder buffer = new StringBuilder();
            buffer.AppendFormat("Packet: len({0})\n", pLength);
            for (int i = 0; i < split.Length; ++i)
            {
                char temp = Convert.ToChar(pBuffer[pStart + i]);
                hex.Append(split[i]).Append(' ');

                if (char.IsWhiteSpace(temp) || char.IsControl(temp))
                {
                    temp = '.';
                }

                ascii.Append(temp);
                if ((i + 1) % 16 == 0)
                {
                    buffer.AppendFormat("{0} {1}", hex, ascii).AppendLine();
                    hex.Clear();
                    ascii.Clear();
                }
            }
            if (hex.Length > 0)
            {
                if (hex.Length < 48)
                {
                    hex.Append(' ', 48 - hex.Length);
                }
                buffer.AppendFormat("{0} {1}", hex, ascii).AppendLine();
            }

            Locker.WaitOne();
            try
            {
                Console.WriteLine(buffer);
            }
            finally
            {
                Locker.ReleaseMutex();
            }
        }

        private static ConsoleColor GetColor(LogLevel pLevel)
        {
            switch (pLevel)
            {
                case LogLevel.Info:
                    return ConsoleColor.Green;
                case LogLevel.Warn:
                    return ConsoleColor.Yellow;
                case LogLevel.Debug:
                    return ConsoleColor.Magenta;
                case LogLevel.Error:
                    return ConsoleColor.DarkRed;
                case LogLevel.Exception:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.White;
            }
        }
    }

    public enum LogLevel
    {
        Default = 0,
        Info,
        Warn,
        Error,
        Exception,
        Debug
    }

}