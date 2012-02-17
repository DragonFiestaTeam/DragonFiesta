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
        public string Map { get; private set; }
        public bool Pending { get; private set; }
        public bool IsOnline { get; set; }
        public byte Month { get; private set; }
        public byte Day { get; private set; }
        public WorldClient  client { get; private set; }

        public static Friend Create(WorldCharacter pCharacter)
        {
       
            Friend friend = new Friend
            {
                ID = pCharacter.Character.ID,
                Name = pCharacter.Character.Name,
                Level = pCharacter.Character.CharLevel,
                Job = pCharacter.Character.Job,
                Map = GetMapname(pCharacter.Character.PositionInfo.Map),
                UniqueID = (uint)pCharacter.Character.AccountID,
                IsOnline = pCharacter.IsIngame,
                client = pCharacter.client,
            };
            
            return friend;
        }
       private static string GetMapname(ushort mapid)
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
                UniqueID = uint.Parse(Row["CharID"].ToString()),
                ID = int.Parse(Row["FriendID"].ToString()),
                Pending = Zepheus.Database.DataStore.ReadMethods.EnumToBool(Row["Pending"].ToString()),
                Day = byte.Parse(Row["LastConnectDay"].ToString()),
                Month = byte.Parse(Row["LastConnectMonth"].ToString()),
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
            this.UniqueID = uint.Parse(Row["CharID"].ToString());
            this.Job = byte.Parse(Row["Job"].ToString());
            this.Level = byte.Parse(Row["Level"].ToString());
            this.Map = GetMapname(ushort.Parse(Row["Map"].ToString()));
        }

        /// <summary>
        /// Updates friend status using a <see cref="WorldCharacter" /> object.
        /// </summary>
        /// <param name="pCharacter">The WorldCharacter object with the new data.</param>
        public void Update(WorldCharacter pCharacter,string name)
        {
            if (ClientManager.Instance.IsOnline(pCharacter.Character.Name))
            {
                if (pCharacter.IsIngame)
                {
                    this.Map = GetMapname(pCharacter.Character.PositionInfo.Map);
                    this.Job = pCharacter.Character.Job;
                    this.Level = pCharacter.Character.CharLevel;
                    if (this.IsOnline) return;
                    Online(name);
                    this.IsOnline = true;
                }
            }
        }
        public void Offline(WorldClient pClient)
        {
            using (var packet = new Packet(SH21Type.FriendOffline))
            {
                packet.WriteString(this.Name, 16);
               pClient.SendPacket(packet);
            }
        }
        public void Online(string Name)
        {
            WorldClient client = ClientManager.Instance.GetClientByCharname(Name);
            if (client == null) return;
            using (var packet = new Packet(SH21Type.FriendOnline))
            {
                packet.WriteString(this.Name, 16);
                packet.WriteString(this.Map, 12);
                client.SendPacket(packet);
            }
        }
        public void UpdatePending(bool Pending)
        {
            Program.DatabaseManager.GetClient().ExecuteQuery("UPDATE SET Pending='"+Pending.ToString()+"' WHERE CharID='"+this.UniqueID+"' AND FriendID='"+this.ID+"'");
            this.Pending = Pending;
        }
        public void WritePacket(Packet pPacket)
        {
            pPacket.WriteBool(IsOnline);	// Logged In
            pPacket.WriteByte(this.Month);	// Last connect Month << 4 (TODO)
            pPacket.WriteByte(this.Day);	// Last connect Day (TODO)
            pPacket.WriteByte(0);	// Unknown (TODO)
            pPacket.WriteString(this.Name, 16);
            pPacket.WriteByte(this.Job);
            pPacket.WriteByte(this.Level);
            pPacket.WriteByte(0);	// In Party (TODO)
            pPacket.WriteByte(0);	// Unkown (TODO)
            pPacket.WriteString(this.Map, 12);
            pPacket.Fill(32, 0);
        }
    }
}
