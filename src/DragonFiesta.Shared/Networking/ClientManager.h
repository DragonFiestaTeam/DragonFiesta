#ifndef CLIENTMANAGER_H
#define CLIENTMANAGER_H

#include "ClientBase.h"
#include "../Util/SecureCollection.hpp"

template<typename T>
class ClientManager
{

public :

    bool AddClient(std::shared_ptr<T> pClient)
    {

        if(!cClientList.TryAdd(pClient->getId(),pClient))
        {
            return false;
        }

        return true;

    }
    bool RemoveClient(std::shared_ptr<T>& pClient)
    {
        if(!cClientList.Remove(pClient))
        {
            return false;
        }
        return true;
    }
    bool RemoveClient(int64 ClientID)
    {
        if(!cClientList.Remove(ClientID))
        {
            return false;
        }

        return true;
    }

    int64 GetCount()
    {
        return    cClientList.size();
    }

private :

    SecureCollection<int64,std::shared_ptr<T>> cClientList;

};

#endif
