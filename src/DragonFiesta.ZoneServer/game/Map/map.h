#ifndef MAP_H
#define MAP_H

#include "game/Enum/MapType.h"
#include "game/Data/MapInfo.h"

class Map
{
    const DragonFiesta::MapType GetType()
    {
        return DragonFiesta::MapType::Map;
    }

     std::shared_ptr<MapInfo>  GetMapInfo() { return mMapInfo; }

    private :

    std::shared_ptr<MapInfo> mMapInfo;

};

#endif
