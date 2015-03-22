#ifndef MOB_H
#define MOB_H

#include "NPC.h"

class Mob : public NPC
{
public :

    const DragonFiesta::EntryType GetType()
    {
        return DragonFiesta::EntryType::MobEntry;
    }

};

#endif
