#ifndef NPC_H
#define NPC_H

#include "MapObject.h"

class NPC : public MapObject
{


public :

    const DragonFiesta::EntryType GetType()
    {
        return DragonFiesta::EntryType::NPCEntry;
    }

};

#endif
