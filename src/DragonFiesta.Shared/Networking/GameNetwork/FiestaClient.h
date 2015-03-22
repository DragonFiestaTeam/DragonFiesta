#ifndef FIESTACLIENT_H
#define FIESTACLIENT_H



#include "FiestaSocket.h"
#include "../Packet/FiestaServerPacket.h"

template<typename T>
class FiestaClient
{
public:
    ~FiestaClient()  { }

    FiestaClient(std::shared_ptr<T> sock)
    {
        //    _Socket = sock;
    }

    bool IsAuthenticated()
    {

        return (GetAuth() && IsConnected());
    }
    bool GetAuth()
    {
        return Authenticated;
    }
    void SetAuth(bool pAuth)
    {
        Authenticated = pAuth;
    }
    bool IsConnected()
    {
        return _Socket->IsOpen();
    }
    std::size_t getId() const
    {
        return (std::size_t) mIdObject.get();
    }

    void SendPacket(FiestaServerPacket pPacket)
    {
//        _Socket.Send(pPacket.contents());
    }
private :
    bool Authenticated;

    std::shared_ptr<T> _Socket;
    std::shared_ptr<int32> mIdObject  = std::make_shared<int32>();

protected:

    T GetSocket()
    {
        return _Socket;
    }

};

#endif
