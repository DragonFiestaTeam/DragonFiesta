#ifndef FACEINFO_H
#define FACEINFO_H

#include "../Enum/BodyShopGrade.h"

class FaceInfo
{

    int8 GetID()
    {
        return ID;
    }
    std::string GetName()
    {
        return Name;
    }
    BodyShopGrade GetGrade()
    {
        return Grade;
    }

private:

    int8 ID;
    std::string Name;
    BodyShopGrade Grade;
};

#endif
