#ifndef ENTRYSPAWNINFO_H
#define ENTRYSPAWNINFO_H

#include "game/Position.h"
#include "game/Data/NPCInfo.h"
#include "game/Enum/NpcFlags.h"

class EntrySpawnInfo
{

public :

    EntrySpawnInfo(int64 _SpawnID)
    {
        SpawnID = _SpawnID;
    }
    private :
    int64 SpawnID;
    int32 NPCID;
    int32 MapID;
    Position Pos;
    NpcType Type;
    NPCInfo  pInfo;

};


#endif
