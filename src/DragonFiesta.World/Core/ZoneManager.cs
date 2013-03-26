using System;
using System.Collections.Generic;
using DragonFiesta.Data;
using DragonFiesta.Messages.World;
using DragonFiesta.Messages.Zone;
using DragonFiesta.InterNetwork;
using DragonFiesta.Util;
using System.Linq;
using System.Threading;

using DragonFiesta.World.Networking;

namespace DragonFiesta.World.Core
{
    public class ZoneManager
    {
        public Dictionary<int,ZoneServer> ActiveZones = new Dictionary<int,ZoneServer>();// ZoneID, Zoneserver
        public static ZoneManager Instance { get; private set; }

        public int ZoneMaxCount { get; private set; }

        public ZoneManager()
        {
            ZoneMaxCount = DataProvider.DataProvider.Instance.maps.Count;
            new Thread(new ThreadStart(Worker)).Start();
        }

        public static bool Initialize()
        {
            try
            {
                Instance = new ZoneManager();
                Logs.Main.Info("Successfully initialized ZoneManager");
                return true;
            }
            catch(Exception ex)
            {
                Logs.Main.Fatal("Could not initialize ZoneManager", ex);
                return false;
            }
        }
        public void RegisterMapToZone(string message)
        {
        }

        public int GetNextZoneID()
        {

            IEnumerable<int> keyRange = Enumerable.Range(0, ZoneMaxCount);
            var freeKeys = keyRange.Except(ActiveZones.Keys);

            return freeKeys.First();
        }

        public ZoneServer GetZone(int id)
        {
            ZoneServer s;
            this.ActiveZones.TryGetValue(id, out s);
            return s;
        }
        public void RegisterZone(ZoneServer pZone)
        {

            ushort NewID = (ushort)GetNextZoneID();
            if (NewID <= ZoneMaxCount)
            {
                pZone.ID = NewID;
                Logs.Main.Debug("Begin Register Zone With ID " + NewID + "");
                pZone.maps = new Dictionary<ushort, Map>();
                foreach (var map in DataProvider.DataProvider.Instance.maps.Values)
                {
                    map.pZoneServer = pZone;
                    pZone.maps.Add(map.ID, map);
                }
                foreach (var map in DataProvider.DataProvider.Instance.maps.Values)
                {
                    if (map.pZoneServer == null)
                    {
                        map.pZoneServer = pZone;
                        pZone.maps.Add(map.ID, map);
                    }
                    else
                    {
                        this.ActiveZones.Add(NewID, pZone);
                        Resizemaps();
                        break;
                    }
                }
                if (!this.ActiveZones.ContainsKey(NewID))
                {
                    pZone.LastPing = DateTime.Now;
                    this.ActiveZones.Add(NewID, pZone);
                }
            }
        }

        public int CaculateZonemapCount()//:TODO fix rest
        {
            int mapcount = DataProvider.DataProvider.Instance.maps.Count;
            int ZoneCount = this.ActiveZones.Count + 1;
            return (int)(mapcount / ZoneCount);
        }
        public void Resizemaps()
        {
            int MapCountPerZone = CaculateZonemapCount();
            int rest = DataProvider.DataProvider.Instance.maps.Count;

            foreach (var zone in this.ActiveZones.Values)
            {
                zone.maps.Clear();
            }
            for (int r = 0; r < DataProvider.DataProvider.Instance.maps.Count; r++)//todo fix rest
            {
                foreach (var zone in this.ActiveZones.Values)
                {
                    if (zone.maps.Count() <= MapCountPerZone)
                    {
                        Map pMap = DataProvider.DataProvider.Instance.maps.Values.ToList()[r];
                        rest--;
                        zone.maps.Add(pMap.ID, pMap);
                        break;
                    }
                }
            }
            SendMapResize();
        }

        public void SendMapResize()
        {
            foreach (var zone in this.ActiveZones.Values)
            {
                RegisterMapList Register = new RegisterMapList
                {
                    Id = Guid.NewGuid(),
                    maps = DataProvider.DataProvider.Instance.maps,
                    ZoneID = zone.ID
                };
                ZoneServer s = ZoneManager.Instance.GetZone(Register.ZoneID);
                ZoneManager.Instance.SendToZone(s, Register);
            }
        }
        public void UnRegisterMapFromZone(string message)
        {
        }
        private void Worker()
        {
            while (true)
            {
                SendZonePingToAll();
                CheckZones();
                Thread.Sleep(10000);
            }
        }
        #region Ping Zones

        public void SendToZone(ZoneServer Zone, IMessage message)
        {
            MessageQueueEx Mx = InternMessageManager.Instance.GetQeueByName(Zone.QeueName);
            if(Mx != null)
            InternMessageManager.Instance.SendSingel(message, Mx);
        }

        public void CheckZones()
        {
            DateTime now = DateTime.Now;

            foreach (var zone in this.ActiveZones.Values)
            {
                if (now.Subtract(zone.LastPing).TotalSeconds >= 30)
                {
                    //remove Zone
                }
            }
        }
        public void UpdatePing(ushort ZoneID)
        {
            ZoneServer Server;
            if (this.ActiveZones.TryGetValue((ushort)ZoneID, out Server))
            {
                Server.LastPing = DateTime.Now;
            }
            else
            {

            }
        }
        private void SendZonePingToAll()
        {
            foreach (var Zone in this.ActiveZones.Values)
            {
                ZonePing ping = new ZonePing
                {
                    Id = Guid.NewGuid(),
                      ZoneID  = Zone.ID
                };
                ZoneServer s = ZoneManager.Instance.GetZone(Zone.ID);
                ZoneManager.Instance.SendToZone(s, ping);
            }
        }
        #endregion

    }
}
