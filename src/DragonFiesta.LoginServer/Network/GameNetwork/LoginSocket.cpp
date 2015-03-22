#include "LoginSocket.h"


LoginSocket::LoginSocket(tcp::socket&& socket) : FiestaSocket(std::move(socket))
{
//std::weak_ptr<LoginClient> pr(std::make_shared<LoginClient>(this));
//pClient(std::make_shared<LoginSocket>(this))
//pClient(std::make_shared<LoginSocket>(this))
}

LoginSocket::~LoginSocket()
{
    std::cout << "Free wird aufgerufen von LoginSocket" << std::endl;
}

void LoginSocket::OnConnect()
{
}

void LoginSocket::OnDisconnect()
{
}


void LoginSocket::HandlePacket(FiestaClientPacket pPacket)
{
//    if(!HandlerStore->CallHandler(pPacket->GetHeader(),pPacket->GetType()))
    //{
//Oh Handler not found ...
  //  }
}
