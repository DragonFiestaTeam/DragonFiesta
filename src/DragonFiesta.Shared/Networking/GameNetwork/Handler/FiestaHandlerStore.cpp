#include "FiestaHandlerStore.h"
#include <utility>


uint16  FiestaHandlerStore::GetFunctionCount()
{
    uint16 FullCount = 0;

    for(auto mH : Handlers)
    {

        FullCount += mH.second->GetHandlerCount();

    }

    return FullCount;

}

uint16 FiestaHandlerStore::GetHandlerCount()
{
    return Handlers.size();
}

void FiestaHandlerStore::AddHandler(std::shared_ptr<ClientHandler> MyHandler)
{
    if(IsHandlerExits(MyHandler->GetHandlerType()) || MyHandler->GetHandlerType() == 0)
    {
        sLog->LogMSG(MSG_WARNING,LTAG_NETWORK,"Invalid HeaderHandler found %i",MyHandler->GetHandlerType());
        return;
    }
    MyHandler->Initialize();
    Handlers.insert(std::make_pair(MyHandler->GetHandlerType(),MyHandler));

}

bool FiestaHandlerStore::GetHandler(uint16 Header,std::shared_ptr<ClientHandler> OutHandler)
{
    if(IsHandlerExits(Header))
    {
        OutHandler = Handlers[Header];
        return true;
    }

    return false;
}
