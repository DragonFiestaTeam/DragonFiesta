#ifndef NPCFLAGS_H
#define NPCFLAGS_H


//default Mob
enum class NpcType
{
    Mob          = 0,
    NPC          = 1,
    Gate         = 2,
};


enum class NpcFlags
{

    None                 = 0,
    ItemVendor           = 1,
    QuestNPC             = 2,
    SoulStoneVendor      = 3,
    WeaponTitleVendor    = 4,
    GuildVendor          = 5,
    SkillVendor          = 6,
    WeaponVendor         = 7

};

#endif
