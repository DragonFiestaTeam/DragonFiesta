#ifndef DATBASE_SERVER_H
#define DATBASE_SERVER_H

#include <map>

#include "Define.h"


class DatabaseServer
{
public :


    DatabaseServer(std::string sHost, uint32 Port, std::string sUser, std::string sPassword);
    DatabaseServer(std::string sUser, std::string sPassword);
    ~DatabaseServer();
    inline std::string GetHost()
    {
        return mHost;
    }
    inline uint32      GetPort()
    {
        return mPort;
    }
    inline std::string GetUser()
    {
        return mUser;
    }
    inline std::string GetPassword()
    {
        return mPassword;
    }

    inline std::string ToString()
    {
        return mUser + "@" + mHost;
    }


private:
    std::string mHost;
    uint32 mPort;

    std::string mUser;
    std::string mPassword;


};


#endif // DATBASE_SERVER_H
