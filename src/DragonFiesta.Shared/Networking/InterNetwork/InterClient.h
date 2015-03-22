#ifndef INTERCLIENT_H
#define INTERCLIENT_H

#include "../ClientBase.h"
#include "InterSocket.h"

class InterClient : public ClientBase<InterSocket>
{

public :
    InterClient(std::shared_ptr<InterSocket> sock);
    InterClient();
    ~InterClient();

};

#endif
