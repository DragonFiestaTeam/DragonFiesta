#ifndef CHARACTERDATA_PROVIDER_H
#define CHARACTERDATA_PROVIDER_H
#include "../../Define.h"

#include "../../Util/SecureWriteCollection.hpp"
#include "../Data/FaceInfo.h"
#include "../Data/HairColorInfo.h"
#include "../Data/HairInfo.h"

class CharacterDataProvider
{

public :
    bool Load();
    bool Unload();
    int64 GetExpForNextLevel(int8 Level);
private :

    bool IsInitialized;
    SecureWriteCollection<int8,uint64> EXPTable;


    SecureWriteCollection<int8,FaceInfo>             FaceInfosByID;
    SecureWriteCollection<int8,HairInfo>             HairInfosByID;
    SecureWriteCollection<int8,HairColorInfo>   HairColorInfosByID;

};

#endif // CHARACTERDATA_PROVIDER_H
