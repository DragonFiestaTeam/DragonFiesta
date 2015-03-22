#ifndef CONSOLE_THREAD
#define CONSOLE_THREAD

#include <thread>
#include "ConsoleCommandHandler.h"


class ConsoleThread
{

public :
    void ReadInput();
    void Stop();
    void Start();
private :
    std::shared_ptr<ConsoleCommandHandler> Handler;
    std::shared_ptr<std::thread> MyThread;
    bool Running;

};

#endif
