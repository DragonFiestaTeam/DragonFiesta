#ifndef FIESTACLIENTPACKET_H
#define FIESTACLIENTPACKET_H

#include "FiestaPacket.h"

class FiestaClientPacket : public FiestaPacket
{
public :
    FiestaClientPacket() = delete;
    FiestaClientPacket(uint16 pHeader,uint16 pType) : FiestaPacket(pHeader,pType)
    {

    }
    uint16 ReadRealHeader()
    {
    }
    uint16 GetHeader()
    {
//        return Header;
    }
    uint16 GetType()
    {
//        return this->Type;
    }
    uint16 GetRealHeader()
    {
  //      return this->RealHeader;
    }

};

#endif
