#ifndef NORMALMAP_H
#define NORMALMAP_H

#include "map.h"
#include "LocalMap.h"
#include "Task/Task.h"

class NormalMap : public LocalMap,public Task
{
public :
    const DragonFiesta::MapType GetType()
    {
        return DragonFiesta::MapType::NormalMap;
    }
    //std::string GetIP() { return ""; }


private :
//Task
    void OnLeave() override;
    void OnAdded() override;

};

#endif
