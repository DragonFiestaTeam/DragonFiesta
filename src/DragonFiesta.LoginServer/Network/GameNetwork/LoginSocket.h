#ifndef LOGINSOCKET_H
#define LOGINSOCKET_H

#include "Networking/GameNetwork/FiestaSocket.h"
#include "LoginClient.h"
#include <boost/asio/ip/tcp.hpp>

class LoginClient;
class LoginSocket : public FiestaSocket
{

    void HandlePacket(FiestaClientPacket pPacket) override;


public :
    LoginSocket(tcp::socket&& socket);
    ~LoginSocket();
    void OnConnect() override;
    void OnDisconnect() override;
protected :
    std::shared_ptr<LoginClient> pClient;
};

#endif
