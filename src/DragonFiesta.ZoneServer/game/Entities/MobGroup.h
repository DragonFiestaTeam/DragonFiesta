#ifndef MOBGROUP_H
#define MOBGROUP_H

#include "Mob.h"
#include "MapObject.h"
#include <list>
#include <memory>

class MobGroup
{

public :



    const DragonFiesta::EntryType GetType()
    {
        return DragonFiesta::EntryType::MobGroupEntry;
    }

    //use for Lua API Later?
    void OnCombat(std::shared_ptr<MapObject> Attacker);
    void OnCombatLeave();

    void Despawn();
    void Spawn();
    void Update();
    bool Load();//TODO FROM DATABASE
    void AddMob(std::shared_ptr<Mob> pMob);
    void RemoveMob(std::shared_ptr<Mob> pMob);
private :



    uint64 GroupID;
    int32 MinMob;
    int32 MaxMob;

    std::list<std::shared_ptr<Mob>> MobList;

};

#endif
