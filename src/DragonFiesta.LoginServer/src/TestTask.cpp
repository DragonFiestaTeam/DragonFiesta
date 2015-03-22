#include "TestTask.h"
#include "Task/TimerTask.h"
#include "Util/Math.h"

TestTask::TestTask() : TimerTask(5000,90000)
{

    this->setCallback(std::bind(&TestTask::Update2, this));
    std::cout << "hier bin ich" << std::endl;
}

void TestTask::Update2()
{
    std::cout << "OnUpDate" << std::endl;

}

TestTask::~TestTask()
{
    //dtor
}
void TestTask::OnAdded()
{

std::cout << "Hello" << std::endl;
}

void TestTask::OnLeave()
{
std::cout << "und schüss" << std::endl;
}
