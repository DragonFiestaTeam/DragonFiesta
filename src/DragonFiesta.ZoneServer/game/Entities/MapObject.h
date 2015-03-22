#ifndef MAPOBJECT_H
#define MAPOBJECT_H

#include "Define.h"
#include "Enum/EntryType.h"

class MapObject
{


public :


    const DragonFiesta::EntryType GetType()
    {
        return DragonFiesta::EntryType::None;
    }

    virtual void Spawn();
};

#endif
