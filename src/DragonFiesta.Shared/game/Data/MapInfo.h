#ifndef MAP_INFO_H
#define MAP_Info_H

#include "../Position.h"

class MapInfo
{
public :


uint32 GetID() { return ID; }
std::string GetName() { return Name; }
std::string GetIndexName() { return IndexName; }
Position GetRegen() { return regen; }

private :

uint32 ID;
std::string Name;
std::string IndexName;
Position regen;


};

#endif
