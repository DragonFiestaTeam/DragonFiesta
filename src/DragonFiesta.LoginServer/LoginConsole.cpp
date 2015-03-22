#include "LoginConsole.h"
#include <iostream>
#include <string>
#include "Console/ConsoleCommandStatus.h"

//TODO LOGGING CLASS FOR THIS SHIT
void LoginConsole::Start()
{
    if(!Running)
    {
        Handler = std::make_shared<ConsoleCommandHandler>();
        Running = true;
        MyThread = std::shared_ptr<std::thread>(new std::thread(&LoginConsole::ReadInput,this));
    }

}

void LoginConsole::Stop()
{
    Running = false;
}

void LoginConsole::ReadInput()
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
