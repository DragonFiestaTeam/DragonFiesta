using System;
using System.Collections.Generic;
using Zepheus.FiestaLib.Data;

namespace Zepheus.Zone.Game
{
	public class Buff
	{
		public DateTime Ends { get; set; }
		public ZoneCharacter Character { get; set; }
		public Dictionary<StatsByte, int> Modifiers { get; set; }
	}
}