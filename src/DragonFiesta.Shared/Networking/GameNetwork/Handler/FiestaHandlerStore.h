#ifndef HANDLER_STORE
#define HANDLER_STORE

#include <map>
#include <functional>
#include "Define.h"
#include "FiestaHandler.h"
#include "Log/Log.h"
#include <memory>

class FiestaHandlerStore
{


public :
    bool IsHandlerExits(uint16 Header)
    {
        auto result = Handlers.find(Header);
        return result != std::end(Handlers);
    }
    void AddHandler(std::shared_ptr<ClientHandler> MyHandler);
    bool CallHandler(uint16 Type,uint16 Header);
    virtual void InitialHandlers() { }
    uint16 GetFunctionCount();
    uint16 GetHandlerCount();




private :

    std::map<uint16, std::shared_ptr<ClientHandler>> Handlers =
    {
        //HandlerType : Header : Handler
    };
    bool GetHandler(uint16 Header,std::shared_ptr<ClientHandler> OutHandler);


};

#endif
