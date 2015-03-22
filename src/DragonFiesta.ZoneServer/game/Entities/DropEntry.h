#ifndef DROPENTRY_H
#define DROPENTRY_H

#include "MapObject.h"

class DropEntry : MapObject
{



    public :
    const DragonFiesta::EntryType GetType()
    {
        return DragonFiesta::EntryType::DropEntry;
    }

};

#endif
