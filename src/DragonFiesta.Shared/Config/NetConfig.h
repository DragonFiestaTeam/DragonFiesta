#ifndef NETCONFIG_CONFIG_H
#define NETCONFIG_CONFIG_H


#include "Define.h"
#include "IO/ini_file.hpp"


class NetConfig
{
public :

    bool                    Read(ini_file::section_map ConfMap);

    std::string             GetListenIP()
    {
        return ListenIP;
    }
    int32                   GetListenPort()
    {
        return ListenPort;
    }
    void                    SetListenPort(int32 pPort)
    {
        ListenPort = pPort;
    }
    void                    SetListenIP(std::string IP)
    {
        ListenIP = IP;
    }
    int32 GetSendBufferSize()
    {
        return SendBufferSize;
    }
    int32 GetSockOutUBuff()
    {
        return SockOutUBuff;
    }
    bool GetTcpNoDelay() { return tcpNoDelay; }


    int32 GetNetworkThreadCount()
    {
        return NetworkThreads;
    }

private :
    std::string             ListenIP;
    int32                   ListenPort;
    int32                   SendBufferSize = -1;
    int32                   SockOutUBuff = 65536;
    bool                    tcpNoDelay = true;
    int32                    NetworkThreads;

};
#endif
