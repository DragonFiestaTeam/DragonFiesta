namespace Zepheus.InterLib.Networking
{
    public enum InterHeader : ushort
    {
        Ping = 0x0000,
        Pong = 0x0001,
        Ivs = 0x0002,
        ClientReady = 0x003,
        ChangeZone = 0x004,
        ClientDisconect = 0x005,
        GetParty = 0x0099,
        SendParty = 0x0100,
        UpdateParty = 0x098,
        RemovePartyMember = 0x097,
        AddPartyMember = 0x0096,
        Auth = 0x0010,
        BanAccount = 0x0095,
        Assign = 0x0100,
        Assigned = 0x0101,

        Clienttransfer = 0x1000,
        Clienttransferzone = 0x1001,

        Zoneopened = 0x2000,
        Zoneclosed = 0x2001,
        Zonelist = 0x2002,

        Worldmsg = 0x3000,
    }
}
