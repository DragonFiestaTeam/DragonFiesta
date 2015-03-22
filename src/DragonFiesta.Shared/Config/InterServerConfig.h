#ifndef INTERSERVER_CONFIG_H
#define INTERSERVER_CONFIG_H

#include "Config/NetConfig.h"
#include "IO/ini_file.hpp"
//TODO OPTIMIZE with NETCONFIG CLass
class InterServerConfig
{
public :
    bool                     Read(ini_file::section_map ConfMap);
    bool                     ReadConnectSettings(ini_file::section_map ConfMap);

    std::string                   GetAuthPassword()
    {
        return AuthPassword;
    }
    std::string                   GetConnectIP()
    {
        return ConnectIP;
    }
    int32                    GetConnectPort()
    {
        return ConnectPort;
    }
    std::string                   GetListenIP()
    {
        return ListenIP;
    }
    int32   GetListenPort()
    {
        return ListenPort;
    }

    int32 GetSendBufferSize()
    {
        return SendBufferSize;
    }
    int32 GetSockOutUBuff()
    {
        return SockOutUBuff;
    }
    bool GetTcpNoDelay()
    {
        return tcpNoDelay;
    }
    int32 GetNetworkThreadCount()
    {
        return NetworkThreads;
    }
private :
    std::string                  AuthPassword;

    std::string                  ConnectIP;
    int32                   ConnectPort;

    std::string                  ListenIP;
    int32                   ListenPort;

    int32                   SendBufferSize = -1;
    int32                   SockOutUBuff = 65536;
    bool                    tcpNoDelay = true;
    int32                    NetworkThreads;

};
#endif
