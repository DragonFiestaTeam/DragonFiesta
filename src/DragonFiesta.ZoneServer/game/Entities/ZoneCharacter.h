#ifndef ZONECHARACTER_H
#define ZONECHARACTER_H

#include "MapObject.h"
#include "game/CharacterBase.h"
//ZoneCharacter : CharacterBase,MapObject


class ZoneCharacter : public CharacterBase,public MapObject
{


public :

    const DragonFiesta::EntryType GetType()
    {
        return DragonFiesta::EntryType::CharacterEntry;
    }

};

#endif
