#ifndef LOCALMAP_H
#define LOCALMAP_H

#include "SectorMap.h"
#include "game/Enum/NpcFlags.h"
#include <list>
#include "game/Data/EntrySpawnInfo.h"
#include "Util/SecureWriteCollection.hpp"

class LocalMap : SectorMap
{
public :
    const DragonFiesta::MapType GetType()
    {
        return DragonFiesta::MapType::LocalMap;
    }

    std::list<EntrySpawnInfo> GetAllSpawnByType(NpcType type);

private :
    SecureWriteCollection<int64,EntrySpawnInfo> SpawnsByID;


};

#endif
