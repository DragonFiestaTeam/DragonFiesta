
namespace Zepheus.FiestaLib
{
    // Named as SHXType , where X = header ID

    public enum SH2Type : byte
    {
        Ping = 4,
        SetXorKeyPosition = 7,
        Chatblock = 72,
        UpdateClientTime = 73,
        UnkTimePacket = 69,
        Unk1 = 14,
    }
    public enum SH19Type : byte
    {
        SendTradeReqest = 2,
        DeclineRequest = 4,
        SendTradeAccept = 9,
        SendTradeBreak = 12,
        SendAddItemSuccefull = 15,
        SendAddItem = 16,
        SendItemRemove = 19,
        SendChangeMoney = 24,
        SendTradeRdy = 27,
        SendTradeLock = 28,
        SendRemoveItemFromHandel = 20,
        SendTradeAgreeMe = 33,
        SendTradeAgreeTo = 34,
        SendTradeComplett = 36,
    }
    public enum SH3Type : byte
    {
        IncorrectVersion = 2, //please update client
        VersionAllowed = 3,
        FilecheckAllow = 5,
        Error = 9,
        WorldlistNew = 10,
        WorldServerIP = 12,
        WorldistResend = 28,

        //Actually used in world
        CharacterList = 20,
    }

    public enum SH4Type : byte
    {
        Money = 51,
        UpdateStats = 53,
        ConnectError = 2,
        Unk = 222,
        ServerIP = 3,
        CharacterGuildinfo = 18,
        CharacterInfo = 56,
        CharacterLook = 57,
        CharacterQuestsBusy = 58,
        CharacterQuestsDone = 59,
        CharacterActiveSkillList = 61,
        CharacterPassiveSkillList = 62,
        CharacterItemList = 71,
        CharacterInfoEnd = 72,
        CharacterTitles = 73,
        CharacterTimedItemList = 74,
        ReviveWindow = 77,
        Revive = 79,
        CharacterPoints = 91,
        SetPointOnStat = 95,
        CharacterGuildacademyinfo = 151,
    }

    public enum SH5Type : byte
    {
        CharCreationError = 4,
        CharCreationOK = 6,
        CharDeleteOK = 12,
    }

    public enum SH6Type : byte
    {
        DetailedCharacterInfo = 2,
        Error = 3,
        RemoveDrop = 5,
        ChangeMap = 9,
        ChangeZone = 10,
        TelePorter = 27,

    }

    public enum SH7Type : byte
    {
        ShowUnequip = 4,
        ShowEquip = 5,
        SpawnSinglePlayer = 6,
        SpawnMultiPlayer = 7,
        SpawnSingleObject = 8,
        SpawnMultiObject = 9,
        ShowDrop = 10,
        ShowDrops = 11,
        RemoveObject = 14,
    }
    public enum SH8Type : byte
    {
        ChatNormal = 2,
        WisperFrom = 13,
        WisperTargetNotfound = 14,
        WisperTo = 15,
        GmNotice = 17,
        StopTele = 19, // Stops char but can teleport
        PartyChat = 21,
        Walk = 24,
        Move = 26,
        Teleport = 27,
        Interaction = 28,
        Shout = 31,
        Emote = 33,
        Jump = 37,
        BeginRest = 40,
        BeginDisplayRest = 41,
        EndRest = 43,
        EndDisplayRest = 44,
        Mounting = 63,
        MapMount = 64,
        Unmount = 66,
        MapUnmount = 67,
        UpdateMountFood = 70,
        CastItem = 71,
        BlockWalk = 74,
    }

    public enum SH9Type : byte
    {
        StatUpdate = 2,
        GainExp = 11,
        LevelUP = 12,
        LevelUPAnimation = 13,
        HealHP = 14,
        HealSP = 15,
        SkillAck = 53,
        ResetStance = 61,
        AttackAnimation = 71,
        AttackDamage = 72,
        DieAnimation = 74,

        SkillUsePrepareSelf = 78,
        SkillUsePrepareOthers = 79, 

        SkillAnimationPosition = 81,
        SkillAnimationTarget = 82,
        SkillAnimation = 87,
    }

    public enum SH12Type : byte
    { 
        ModifyItemSlot = 1,
        ModifyEquipSlot = 2,
        InventoryFull = 4,
        ObtainedItem = 10,
        FailedEquip = 17,
        FailedUnequip = 19,
        ItemUseEffect = 22,
        ItemUpgrade = 24,
        ItemUsedOk = 26,
        SendPremiumItemList = 33,
        SendRewardList = 45,
    }
    public enum SH14Type : byte
    {
        // According to my informations, 7 is InviteDeclined.
        // NOTE - IT IS.
		// Header 7 somehow changed I guess?
		// seems to be answer or related to CH14::72
		// new data is CHAR[16] NAME | USHORT UNK (C1 04)
		// purpose complete unknown
        InviteDeclined = 7,
        UpdatePartyMemberLoc = 73,
        UpdatePartyMemberStats = 50,
        SetMemberStats = 51,
		// Invite/Accept might be switched up
        PartyInvite = 3,
        PartyAccept = 4,
        PartyDropState = 76,
        PartyList = 9,
		// changed.
        PartyLeave = 11,
        GroupList = 85,
		// might changed as well?
        ChangePartyMaster = 41,
        ChangePartyDrop = 75,
        KickPartyMember = 21,

        BreakUp = 30,

		// COMPLETE UNKOWN
		// DATA: CHAR[16] NAME, thats it.
		UNK_1 = 71,
    }
    public enum SH15Type : byte
    {
        Question = 1,
        HandlerWeapon = 9,
        HanlderSkill = 10,
        HandlerStone = 5,
        HandlerTitel = 11,
        GuildNpcReqest = 12,
    }

    //skills & crap?
    public enum SH18Type : byte
    {
        LearnSkill = 4,
    }

    public enum SH20Type : byte
    {
        ChangeHPStones = 3,
        ChangeSPStones = 4,
        ErrorBuyStone = 5,
        ErrorUseStone = 6,
        StartHPStoneCooldown = 8,
        StartSPStoneCooldown = 10,
    }
    public enum SH21Type : byte
    {
        FriendListDelete = 6,
        FriendInviteResponse = 2,
        FriendInviteRequest = 3,
        FriendExtraInformation = 8,
        FriendOnline = 9,
        FriendOffline = 10,
        FriendInviteReject = 11,
        FriendDeleteSend = 12,
        FriendChangeMap = 13,
    }
    public enum SH25Type : byte
    {
        WorldMessage = 2,
    }

    public enum SH28Type : byte
    {
        LoadQuickBar = 3,
        LoadQuickBarState = 5,
        LoadGameSettings = 11,
        LoadClientSettings = 13,
        LoadShortCuts = 15,
    }

    public enum SH29Type : byte
    {
        SendRequesterResponse = 10,
        SendGuildInvideRequest = 11,
        GuildKickResponse = 15,
        ChangeGuildMessageResponse = 17,
        RankChangeResponse = 23,
        LeaveResponse = 29,
        ClearGuildDetailsMessage = 191,
        UnkMessageChange = 196,
        GuildList = 27,
        RemoveFromGuild = 29,
        ChangeResponse = 39,
        SendUpdateDetails = 45,
        AddGuildMember = 54,
        GuildMemberKick = 55,
        RemoveGuildMember = 56,
        ChangeRank = 57,
        SendMemberGoOnline = 61,
        SendMemberGoOffline = 62,
        GuildChatMessage = 116,
        GuildNameResult = 119,

    }
    public enum SH37Type : byte
    {
        SendMasterRequestAccept = 3,
        SendMasterRequestReponse = 2,
        SendMasterRequest = 4,
        SendMasterResponseRemove = 7,
        SendRemoveMember = 11,
        SendMasterList = 20,
        SendRegisterApprentice = 21,
        SendMasterMemberOnline = 22,
        SendMasterMemberOffline = 23,
        SendApprenticeRemoveMaster = 24,
        SendApprenticeLevelUp = 25,
        SendApprenticeReward = 26,
        SendRecivveCopper = 61,
        SendGiveMasterReward = 65,
        MasterReiveCopperDecline = 69,
    
    }
    public enum SH38Type : byte
    {
        GuildAcademyDetails = 8,// Send When Join
        GuildAcademyList = 14,
        GuildAcademyRequest = 18,
        GuildAcademyJoin = 19,
        GuildAcademyChangeDetailsResponse = 37,
        GuildAcademyChangeDetails = 38,
        KickResponse = 23,
        KickGuildAcademyMember = 24,
        GuildAcademyLeaveResponse = 28,//6052 = failed becouase hour 6016 // Succfuell
        BlockMessage = 35,
        ChangeResponsePacket = 42,
        ChangeRequest = 43,
        JoinGuildFromAcademy = 46,
        GuildAcademyDekanChange = 91,
        GuildAcademyMemberLeave = 96,
        GuildAcademyMemberLoggetOn = 97,
        GuildAcademyMemberOffline = 98,
        GuildAcademyMemberChangeMap = 99,
        GuildAcademyMemberLevelUp = 102,
        GuildAcademMemberChangeJob = 103,
        GuildAcademyChatessage = 105,
        ChatBlock = 106,
        GuildAcademyReward = 115,
    }
    public enum SH42Type : byte
    {
        BlockList = 2,
        AddToBlockList = 6,
        RemoveFromBlockList = 10,
        ClearBlockList = 14,
    }
    public enum SH31Type : byte
    {
        LoadUnkown = 7,
    }
}
