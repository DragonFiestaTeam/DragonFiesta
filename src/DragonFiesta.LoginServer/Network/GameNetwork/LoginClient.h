#ifndef LOGINCLIENT_H
#define LOGINCLIENT_H

#include <memory>
#include "Networking/GameNetwork/FiestaClient.h"
#include "Network/GameNetwork/LoginSocket.h"

class LoginSocket;
class LoginClient  : public FiestaClient<LoginSocket>
{
public :
    LoginClient(std::shared_ptr<LoginSocket> sock) : FiestaClient<LoginSocket>(std::move(sock)),_Sock(sock)
    {
    }
  //  LoginClient();
  std::weak_ptr<LoginSocket> _Sock;
    ~LoginClient() { }
};

#endif
