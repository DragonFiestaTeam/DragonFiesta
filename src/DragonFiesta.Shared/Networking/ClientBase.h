#ifndef CLIENTBASE_H
#define CLIENTBASE_H

#include "Socket.h"
#include <memory>

template<class T>
class ClientBase
{

public :
  //  ClientBase() = delete;

    ClientBase(std::shared_ptr<Socket<T>> sock)
    {
        _Socket = sock;
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
        return _Socket->getId();
    }
    //using Later when finished :D
    //virtual void SendPacket() = 0;
    //virtual void Send() = 0

private :



    bool Authenticated;

    std::shared_ptr<Socket<T>> _Socket;


protected:

    T GetSocket()
    {
        return _Socket;
    }

};

#endif
