#ifndef __FIESTASOCKET_H
#define __FIESTASOCKET_H

#include "Networking/Socket.h"
#include "FiestaClient.h"
#include "../Packet/FiestaClientPacket.h"
#include "../Packet/FiestaServerPacket.h"

union ClientPktHeader
{
    struct
    {
        uint32 Command;
        uint32 Size;
    } Normal;

    static bool IsValidSize(uint32 size)
    {
        return size < 10240;
    }
    static bool IsValidOpcode(uint32 opcode)
    {
        return opcode < 9000;
    }
};

class FiestaSocket : public Socket<FiestaSocket>
{
public:
    FiestaSocket(tcp::socket&& socket);
    virtual ~FiestaSocket();
    FiestaSocket(FiestaSocket const& right) = delete;
    FiestaSocket& operator=(FiestaSocket const& right) = delete;
    void Start() override;
    // void SendPacket(WorldPacket const& packet);
    bool GetHandShaking()
    {
        return HandShaked;
    }
    virtual void OnConnect() = 0;
    virtual void OnDisconnect() = 0;
    virtual void HandlePacket(FiestaClientPacket pPacket) = 0;
    void SendPacket(std::shared_ptr<FiestaServerPacket> pPacket);
    void OnOpen();
    void OnClose();
protected:
    void ReadHandler() override;
    bool ReadHeaderHandler();
    bool ReadDataHandler();
private:

    std::shared_ptr<uint32> XorPos;
    bool HandShaked;
    void SendHandShake();
    void WritePacketToBuffer(std::shared_ptr<FiestaServerPacket> pPacket,MessageBuffer& pBuffer);
    MessageBuffer _headerBuffer;
    MessageBuffer _packetBuffer;
};

#endif
