using System;
using System.Collections.Generic;
using DragonFiesta.InterNetwork;
using DragonFiesta.Data;
using DragonFiesta.DataProvider;


namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class CharacterReadyToLogin : IMessage
    {
        public Guid Id { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public bool ReadyOK { get; set; }
        public ushort MapID { get; set; }
    }
}
