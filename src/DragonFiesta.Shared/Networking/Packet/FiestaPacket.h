#ifndef FIESTAPACKET_H
#define FIESTAPACKET_H

#include "PacketStream.h"
#include "ReadStream.h"
#include "WriteStream.h"

class FiestaPacket : public PacketStream
{
public :
    FiestaPacket() = delete;
    FiestaPacket(uint16 Header,uint16 Type)
    {
        pHeader = Header;
        pType = Type;
    }

    void PrintRealHeader();
    void PrintHeaderAndType();

protected :
    uint16 pHeader;
    uint16 pType;
    uint16 pRealHeader;

    void SetRealHeader(uint16 Header);
};

#endif
