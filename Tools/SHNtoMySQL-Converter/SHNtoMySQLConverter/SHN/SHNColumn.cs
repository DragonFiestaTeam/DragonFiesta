using System;
using System.Data;
using System.IO;

namespace SHNtoMySQLConverter.SHN
{
    public class SHNColumn : DataColumn
    {
        public int Length { get; private set; }
        public byte TypeByte { get; private set; }
        public string MYSQLType { get; private set; }

        public void Load(SHNReader reader, ref int unkcount)
        {
            string caption = reader.ReadPaddedString(48);
            if (caption.Trim().Length < 2)
            {
                base.ColumnName = "Undefined " + unkcount.ToString();
                ++unkcount;
            }
            else
            {
                base.ColumnName = caption;
            }
            this.TypeByte = (byte)reader.ReadUInt32();
            this.DataType = GetType(this.TypeByte);
            this.MYSQLType = GetMYSQLType(this.TypeByte);
            this.Length = reader.ReadInt32();
        }

        public void Write(SHNWriter writer)
        {
            if (this.ColumnName.StartsWith("Undefined"))
            {
                writer.WritePaddedString(" ", 48);
            }
            else
            {
                writer.WritePaddedString(this.ColumnName, 48);
            }
            writer.Write((uint)this.TypeByte);
            writer.Write((uint)this.Length);
        }

        public static Type GetType(uint pCode)
        {
            switch (pCode)
            {
                case 1:
                case 12:
                    return typeof(byte);
                case 2:
                    return typeof(UInt16);
                case 3:
                case 11:
                    return typeof(UInt32);
                case 5:
                    return typeof(Single);
                case 0x15:
                case 13:
                    return typeof(Int16);
                case 0x10:
                    return typeof(byte);
                case 0x12:
                case 0x1b:
                    return typeof(UInt32);
                case 20:
                    return typeof(SByte);
                case 0x16:
                    return typeof(Int32);
                case 0x18:
                case 0x1a:
                case 9:
                    return typeof(string);
                default:
                    return typeof(object);
            }
        }

        public string GetMYSQLType(uint pCode)
        {

            switch (pCode)
            {
                case 1:
                case 12:
                case 16:
                    return " int(11) unsigned NOT NULL ";

                case 2:
                    return " int(11) unsigned NOT NULL ";

                case 3:
                case 11:
                case 18:
                case 27:
                    return " int(11) unsigned NOT NULL ";

                case 5:
                    return " int(11) unsigned NOT NULL ";

                case 9:
                case 24:
                    return " text NOT NULL ";

                case 13:
                case 21:
                    return " int(11) unsigned NOT NULL ";

                case 20:
                    return " int(11) unsigned NOT NULL ";

                case 22:
                    return " int(11) unsigned NOT NULL ";

                case 26:       // TODO: Should be read until first null byte, to support more than 1 this kind of column
                    return " text NOT NULL ";

                default:
                    throw new Exception("New column type found");
                /*                case 1:
                case 12:
                    return " tinyint(3) unsigned ";//typeof(byte);
                case 2:
                    return " smallint(5) unsigned NOT NULL ";//typeof(UInt16); 
                case 3:
                case 11:
                    return " tinyint(2) unsigned NOT NULL ";//typeof(UInt32);
                case 5:
                    return " float NOT NULL ";//typeof(Single);
                case 0x15:
                case 13:
                    return " int(11) NOT NULL ";//typeof(Int16);
                case 0x10:
                    return " tinyint(3) unsigned ";//typeof(byte);
                case 0x12:
                case 0x1b:
                    return " int(11) unsigned NOT NULL ";//typeof(UInt32);
                case 20:
                    return " tinyint(3) ";//typeof(SByte);
                case 0x16:
                    return " int(11) NOT NULL ";//typeof(Int32);
                case 0x18:
                case 0x1a:
                case 9:
                    return " text NOT NULL ";//typeof(string);
                default:
                    return " unknown.. ";//typeof(object); */
            }
        }
    }
}
