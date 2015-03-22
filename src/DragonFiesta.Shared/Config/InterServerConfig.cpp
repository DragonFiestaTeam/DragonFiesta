#include "InterServerConfig.h"
#include "Log/Log.h"


bool InterServerConfig::Read(ini_file::section_map ConfMap)
{
    try
    {
        ListenIP    = ConfMap["InterNetwork"]["ListenIP"];
        ListenPort  = std::stoi(ConfMap["InterNetwork"]["ListenPort"]);
        SendBufferSize  = std::stoi(ConfMap["InterNetwork"]["SendBufferSize"]);
        SockOutUBuff = std::stoi(ConfMap["InterNetwork"]["SockOutUBuff"]);
        NetworkThreads = std::stoi(ConfMap["InterNetwork"]["Threads"]);
        if(ListenIP == "" || ListenPort == 0 || SendBufferSize == 0 || SockOutUBuff == 0 || NetworkThreads == 0)
        {
            sLog->LogMSG(MSG_ERROR,"Failed to read InterNetwork ListenIP or Port or InterLog settings are is missing! Please Check you InterNetwork Settings");
            return false;
        }

        return true;
    }
    catch(...)
    {

        sLog->LogMSG(MSG_ERROR,"Failed to read InterNetwork Config Please Check in Section InterNetwork you InterNetwork Settings");
        return false;
    }

}

bool InterServerConfig::ReadConnectSettings(ini_file::section_map ConfMap)
{

    try
    {
        ConnectIP   = ConfMap["InterNetwork"]["ConnectIP"];
        ConnectPort = std::stoi(ConfMap["InterNetwork"]["ConnectPort"]);

        SendBufferSize  = std::stoi(ConfMap["InterNetwork"]["SendBufferSize"]);
        SockOutUBuff = std::stoi(ConfMap["InterNetwork"]["SockOutUBuff"]);
        NetworkThreads = std::stoi(ConfMap["InterNetwork"]["Threads"]);
        if(ConnectIP == "" || ConnectPort == 0 || SendBufferSize == 0 || SockOutUBuff == 0 || NetworkThreads == 0)
        {
            sLog->LogMSG(MSG_ERROR,"Failed to read InterNetwork ConnectIP or ConnectPort is are missing! Please Check you InterNetwork Settings");
            return false;
        }
        return true;
    }
    catch(...)
    {
        sLog->LogMSG(MSG_ERROR,"Failed to read InterNetwork ConnectIP or ConnectPort is are missing! Please Check you InterNetwork Settings");
        return false;
    }


}
