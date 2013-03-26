using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DragonFiesta.Login.DataTypes;
using DragonFiesta.Messages;
using DragonFiesta.Messages.World;
using DragonFiesta.Util;
using Zepheus.FiestaLib;

namespace DragonFiesta.Login.Core
{
	public class WorldManager
	{
		#region .ctor

		public WorldManager()
		{
			Servers = new List<WorldServerInfo>();
		}

		#endregion
		#region Properties

		public static WorldManager Instance { get; private set; }

		public List<WorldServerInfo> Servers { get; private set; }

	    private int nextId = 0;

		#endregion
		#region Methods

		public static void Initialize()
		{
			Instance = new WorldManager();
		}

		public void UpdateWorldServerStatus(int pId, WorldStatus pStatus)
		{
			WorldServerInfo info = Servers.FirstOrDefault(i => i.ID == pId);
			if(info == null)
				return;	// Throw? (NoSuchServer)
			info.UpdateStatus(pStatus);
		}

        public int RegisterServer(NewWorldServerStarted msg)
        {
            WorldServerInfo info = new WorldServerInfo(nextId, nextId.ToString(CultureInfo.InvariantCulture), msg.Ip, msg.Port, WorldStatus.Low);
            nextId++;
            Servers.Add(info);
            return info.ID;
        }

		#endregion
	}
}
