#ifndef FIESTAHANDLER_H
#define FIESTAHANDLER_H
#include <functional>
#include <map>
#include "Define.h"
#include "Log/Log.h"
#include "Networking/GameNetwork/FiestaClient.h"
#include "Networking/Packet/FiestaPacket.h"
#include <unordered_map>
//typedef std::function<void(FiestaClient,FiestaClientPacket)> HandlerFunc;

class FiestaHandler
{

public :
    virtual uint16 GetHandlerType()
    {
        return 0;
    }

};

class ClientHandler  :  public FiestaHandler
{
public :
//TODO defining behavior
    void AddHandler(uint16 Header, std::function<void (FiestaServerPacket)>& func);
    bool IsHandlerExits(uint16 Type);
    virtual void Initialize() { }
    uint16 GetHandlerCount();
    std::function<void (FiestaServerPacket)> GetHandlerMethod(uint16 Header);

private :
    std::map<uint16, std::function<std::function<void (FiestaServerPacket)>>> Handler;

};

class ServerHandler  : public FiestaHandler
{

public :



};

#endif
