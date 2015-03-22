#ifndef REMOTEMAP_H
#define REMOTEMAP_H

#include "Map/map.h"
#include "Config/NetConfig.h"

class RemoteMap : public Map
{
public :
    const DragonFiesta::MapType GetType()
    {
        return DragonFiesta::MapType::RemoteMap;
    }

    std::shared_ptr<NetConfig> NetConf;
};
#endif
