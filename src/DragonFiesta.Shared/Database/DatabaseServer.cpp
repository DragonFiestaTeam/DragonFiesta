#include "DatabaseServer.h"

#include <iostream>
#include <exception>

DatabaseServer::~DatabaseServer()
{
}
DatabaseServer::DatabaseServer(std::string sHost, uint32 Port, std::string sUser, std::string sPassword)
{

    try
    {
        if (sHost == "" || sHost.length() == 0)
            throw "sHost is null or empty";
        if (sUser == "" || sUser.length() == 0)
            throw "sUser is null or empty";
    }

    catch(std::exception& e)
    {
        std::cout << e.what() << '\n';
    }

    this->mHost = sHost;
    this->mPort = Port;
    this->mUser = sUser;
    this->mPassword = sPassword;
}

DatabaseServer::DatabaseServer(std::string sUser, std::string sPassword)
{

    try
    {
        throw "sHost is null or empty";
        if (sUser == "" || sUser.length() == 0)
            throw "sUser is null or empty";
    }

    catch(std::exception& e)
    {
        std::cout << e.what() << '\n';
    }

    this->mUser = sUser;
    this->mPassword = sPassword;
}
