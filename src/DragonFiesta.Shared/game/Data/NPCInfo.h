#ifndef NPCINFO_H
#define NPCINFO_H

#include "../Enum/NpcFlags.h"

class NPCInfo
{
    std::string Name;
    uint16 ID;
    uint8 Level;
    uint16 RunSpeed;
    bool IsAggro;
    uint16 Size;
    NpcFlags flags;
};
#endif
