#ifndef __INTERSOCKET_H
#define __INTERSOCKET_H

#include "Networking/Socket.h"

union ClientPktHeader
{
    struct
    {
        uint32 Command;
        uint32 Size;
    } Normal;

    static bool IsValidSize(uint32 size) { return size < 10240; }
    static bool IsValidOpcode(uint32 opcode) { return opcode < 9000; }
};

class InterSocket : public Socket<InterSocket>
{
public:
    InterSocket(tcp::socket&& socket);
    ~InterSocket();
    InterSocket(InterSocket const& right) = delete;
    InterSocket& operator=(InterSocket const& right) = delete;
    void Start() override;
    void OnOpen();
    void OnClose();
   // void SendPacket(WorldPacket const& packet);

protected:
    void ReadHandler() override;
    bool ReadHeaderHandler();
    bool ReadDataHandler();

private:



    //WorldSession* _worldSession;
    MessageBuffer _headerBuffer;
    MessageBuffer _packetBuffer;

};

#endif
