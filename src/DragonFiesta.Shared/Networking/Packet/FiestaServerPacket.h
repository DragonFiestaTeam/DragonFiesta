#ifndef FIESTASERVERPACKET_H
#define FIESTASERVERPACKET_H

#include "FiestaPacket.h"
#include "WriteStream.h"

class FiestaServerPacket final : public FiestaPacket,public WriteStream
{
public :
    FiestaServerPacket() = delete;
    FiestaServerPacket(uint16 pHeader,uint16 pType) : FiestaPacket(pHeader,pType)
    {
    WriteRealHeader();
    }

    void WriteRealHeader()
    {
    }

};


#endif
