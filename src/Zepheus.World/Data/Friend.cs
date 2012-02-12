using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zepheus.Database;
using Zepheus.World.Networking;
using Zepheus.FiestaLib;
using Zepheus.FiestaLib.Networking;
using Zepheus.FiestaLib.Data;

namespace Zepheus.World.Data
{
    public class Friend
    {
        public uint UniqueID { get; set; }
        public int ID { get; private set; }
        public string Name { get; private set; }
        public byte Level { get; private set; }
        public byte Job { get; private set; }
        public ushort Map { get; private set; }
        public bool Pending { get; private set; }
        public bool IsOnline { get; set; }

        public static Friend Create(WorldCharacter pCharacter)
        {

            Friend friend = new Friend
            {
                ID = pCharacter.Character.ID,
                Name = pCharacter.Character.Name,
                Level = pCharacter.Character.CharLevel,
                Job = pCharacter.Character.Job,
                Map = pCharacter.Character.PositionInfo.Map,
                IsOnline = true
            };

            return friend;
        }
        private string GetMapname(ushort mapid)
        {
            MapInfo mapinfo;
            if (DataProvider.Instance.Maps.TryGetValue(mapid, out mapinfo))
            {
                return mapinfo.ShortName;
            }
            return "";
        }
        public static Friend LoadFromDatabase(DataRow Row)
        {
            Friend friend = new Friend
            {
                UniqueID = uint.Parse(Row["ID"].ToString()),
                ID = int.Parse(Row["FriendID"].ToString()),
                Pending = Zepheus.Database.DataStore.ReadMethods.EnumToBool(Row["Pending"].ToString()),
            };
            return friend;
        }
        /// <summary>
        /// Updates friend status with input from characters table
        /// </summary>
        /// <param name="pReader"></param>
        public void UpdateFromDatabase(DataRow Row)
        {
            this.Name = Row["Name"].ToString();
            this.Job = byte.Parse(Row["Job"].ToString());
            this.Level = byte.Parse(Row["Level"].ToString());
            this.Map = ushort.Parse(Row["Map"].ToString());
        }

        /// <summary>
        /// Updates friend status using a <see cref="WorldCharacter" /> object.
        /// </summary>
        /// <param name="pCharacter">The WorldCharacter object with the new data.</param>
        public void Update(WorldCharacter pCharacter)
        {
            this.Map = pCharacter.Character.PositionInfo.Map;
            this.Job = pCharacter.Character.Job;
            this.Level = pCharacter.Character.CharLevel;

        }
        public void WritePacket(Packet pPacket)
        {
            pPacket.WriteBool(IsOnline);	// Logged In
            pPacket.WriteByte(0);	// Last connect Month << 4 (TODO)
            pPacket.WriteByte(0);	// Last connect Day (TODO)
            pPacket.WriteByte(0);	// Unknown (TODO)
            pPacket.WriteString(this.Name, 16);
            pPacket.WriteByte(this.Job);
            pPacket.WriteByte(this.Level);
            pPacket.WriteByte(0);	// In Party (TODO)
            pPacket.WriteByte(0);	// Unkown (TODO)
            pPacket.WriteString(GetMapname(this.Map), 12);
            pPacket.Fill(32, 0);
        }
    }
}
