#include "FiestaHandler.h"
#include <utility>
bool ClientHandler::IsHandlerExits(uint16 Header)
{
  //  auto result = Handler.find(Header);
//    return result != std::end(Handler);
}

uint16 ClientHandler::GetHandlerCount()
{
    return Handler.size();
}

void ClientHandler::AddHandler(uint16 Header, std::function<void (FiestaServerPacket)>& func)
{
    if(ClientHandler::IsHandlerExits(Header) || Header != 0)
    {
        sLog->LogMSG(MSG_WARNING,LTAG_NETWORK,"Invalid FiestaHandler Found %i",Header);
        return;
    }
    //  Handler.insert(std::make_pair(Header,func));
}

std::function<void (FiestaServerPacket)> ClientHandler::GetHandlerMethod(uint16 Header)
{
    if(ClientHandler::IsHandlerExits(Header))
    {
       // OutFunc = *Handler[Header];
        //return true;

//        return func;
    }
    return nullptr;
}
