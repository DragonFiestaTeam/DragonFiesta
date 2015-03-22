#include "ConsoleThread.h"
#include "ConsoleCommandStatus.h"
#include <iostream>
#include <string>

//TODO LOGGING CLASS FOR THIS SHIT
void ConsoleThread::Start()
{
    if(!Running)
    {
        Handler = std::make_shared<ConsoleCommandHandler>();
        Running = true;
        MyThread = std::shared_ptr<std::thread>(new std::thread(&ConsoleThread::ReadInput,this));
    }

}

void ConsoleThread::Stop()
{
}

void ConsoleThread::ReadInput()
{

    while(Running)
    {
        std::string input;

        std::getline(std::cin,input);
        switch(Handler->ExecuteCommand(input))
        {
        case ConsoleCommandStatus::Error:
            std::cout << "ConsoleCommand Failed to Execute" << std::endl;
        case ConsoleCommandStatus::NotFound:
            std::cout << "Command not found" << std::endl;
        case ConsoleCommandStatus::Done:
        continue;
        }
    }

}
