#ifndef KINGDOMMAP_H
#define KINGDOMMAP_H

#include "LocalMap.h"
#include "Task/TimerTask.h"

class KingdomMap : LocalMap,TimerTask
{

    const DragonFiesta::MapType GetType()
    {
        return DragonFiesta::MapType::KingdomMap;
    }

    void OnLeave() override;
    void OnAdded() override;

};
#endif
