#ifndef GATENPC_H
#define GATENPC_H

class GateNPC : NPC
{

    const DragonFiesta::EntryType GetType()
    {
        return DragonFiesta::EntryType::GateEntry;
    }

};

#endif
