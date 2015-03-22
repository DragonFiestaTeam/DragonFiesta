#include "DatabaseManager.hpp"
#include "Task/TaskManager.h"
#include "Task/Timer.h"
#include "Log/Log.h"
#include "DatabaseClientState.h"
#include <functional>
#include <memory>


DatabaseManager::DatabaseManager(std::shared_ptr<Database> _DB,std::shared_ptr<DatabaseServer> DBServer) : Task(5000)//5 second
{
    this->setCallback(std::bind(&DatabaseManager::UpdateClients, this));


    mClients.reserve(_DB->GetMaxPoolSize());

    this->mDatabase = _DB;
    this->mServer = DBServer;


    //pushing dummy -.-
    for (uint32 i = 0; i < _DB->GetMaxPoolSize(); i++)
    {
        mClients.push_back(nullptr);
    }
}
void DatabaseManager::OnAdded()
{

    this->PrintMysqlInfos();

}

void DatabaseManager::OnLeave()
{

//Here shutdown Logic :D
}

void DatabaseManager::PrintMysqlInfos()
{
    sLog->LogMSG(MSG_INFO,LTAG_DATABASE,"Found  MysqlClientVersion : %s ", mysql_get_client_info());

    sLog->LogMSG(MSG_INFO,LTAG_DATABASE,"MaxPoolSize : %d MinPoolSize : %d Database : %s ",
                 this->mDatabase->GetMaxPoolSize(),
                 this->mDatabase->GetMinPoolSize(),
                 this->mDatabase->GetName().c_str());
}

uint32 DatabaseManager::GetConnectionCount()
{
    uint32 Counter = 0;
    for (uint32 i = 0; i < mClients.size(); i++)
    {
        auto Client = mClients[i];
        if (Client == nullptr)
        {
            continue;
        }
        if(Client->State == Closed)
        {
            continue;
        }

        if (Client->State != None)
        {
            Counter++;
        }
    }

    return Counter;
}

void DatabaseManager::StopMonitoring()
{
    TaskManager::GetInstance().RemoveTask(this->getId());
}

void DatabaseManager::ClientConnecting(uint32 HandleID)
{
    if(mClients[HandleID] != nullptr)
    {
        mClients[HandleID]->Connect(
            mServer->GetHost(),
            mServer->GetUser(),
            mServer->GetPassword(),
            mDatabase->GetName());

        sLog->LogMSG(MSG_DEBUG,LTAG_DATABASE,"Opening Database Connection With Client # %d",mClients[HandleID]->mHandle());
    }

}

void DatabaseManager::ClientDisconnecting(uint32 HandleID)
{

    if(mClients[HandleID] != nullptr)
    {
        mClients[HandleID]->Disconnect();
        sLog->LogMSG(MSG_DEBUG,LTAG_DATABASE,"Disconnecting Client %d",HandleID);
    }

}

void DatabaseManager::DestroyClients()
{

    for (uint32 i = 0; i < mClients.size(); i++)
    {
        if(mClients[i] != nullptr)
        {
            mClients[i]->Destroy();
            mClients[i] = nullptr;
        }

    }
}

void DatabaseManager::UpdateClients()
{
    try
    {
        for (uint32 i = 0; i < mClients.size(); i++)
        {
            if(mClients[i] == nullptr) continue;
            if((mClients[i]->State == Idle || mClients[i]->State  == Broken || GetMSTimeDiffToNow(mClients[i]->LastActive) >= 60000))   //closing connection by nothing activity
            {
                if(mClients[i]->State == Broken || GetConnectionCount() > mDatabase->GetMinPoolSize())
                {
                    this->ClientDisconnecting(i);
                }
                else
                {
                    sLog->LogMSG(MSG_DEBUG,LTAG_DATABASE,"Renew Lifetime for DatabaseClient# %d",i);
                    this->mClients[i]->UpdateLastActivity();
                }

            }

        }
    }
    catch(...) {}

}

std::shared_ptr<DatabaseClient> DatabaseManager::GetClient()
{
    mMutex.lock();

    uint32 connectionCount = GetConnectionCount();

    if(mDatabase->GetMinPoolSize() > connectionCount)
    {

        auto clientitr = std::find(mClients.begin(), mClients.end(), nullptr);
        uint32 HandleID =  distance (mClients.begin (), clientitr);

        mClients[HandleID] = std::make_shared<DatabaseClient>(HandleID);
        this->ClientConnecting(HandleID);
        mClients[HandleID]->State = Busy;
        mClients[HandleID]->UpdateLastActivity();
        mMutex.unlock();
        return mClients[HandleID];

    }

    for (uint32 i = 0; i < mClients.size(); i++)
    {

        if(mClients[i] == nullptr)
        {

            mClients[i] = std::make_shared<DatabaseClient>(i);
            this->ClientConnecting(i);
            mClients[i]->State = Busy;
            mClients[i]->UpdateLastActivity();
            sLog->LogMSG(MSG_DEBUG,LTAG_DATABASE,"Realease new DatabaseClient# %d",i);
            mMutex.unlock();
            return mClients[i];
        }

        switch(mClients[i]->State)
        {

        case Busy: //oh Client is Bussy No Problem Continue to next :D
            continue;
            break;

        case Idle : //Using Ready Client Again :D
            mClients[i]->UpdateLastActivity();
            mClients[i]->State = Busy;
            sLog->LogMSG(MSG_DEBUG,LTAG_DATABASE,"Realease idle DatabaseClient# %d",i);
            mMutex.unlock();
            return mClients[i];
            break;

        case Broken : //Reagain :D

            mClients[i]->Destroy();
            mClients[i] = std::make_shared<DatabaseClient>(i);
            this->ClientConnecting(i);
            if(mClients[i]->State == Idle)
            {
                sLog->LogMSG(MSG_DEBUG,LTAG_DATABASE,"Realease Broken DatabaseClient# %d",i);
                mMutex.unlock();
                return mClients[i];
            }
            break;

        case Closed :
            ClientConnecting(i);
            mClients[i]->UpdateLastActivity();
            mClients[i]->State = Busy;
            sLog->LogMSG(MSG_DEBUG,LTAG_DATABASE,"Realease closed DatabaseClient# %d",i);
            mMutex.unlock();
            return mClients[i];
            break;
        default:
            sLog->LogMSG(MSG_WARNING,LTAG_DATABASE,"Unkown DatabaseClient HandleType for ID %d ",i);
            break;
        }

    }

    if(connectionCount >= mDatabase->GetMaxPoolSize())
    {
        sLog->LogMSG(MSG_WARNING,LTAG_DATABASE,"Mysql Pool Overloaded! ");
    }

    mMutex.unlock();

    return nullptr;
}

my_ulonglong DatabaseManager::runCommand(const char* const command)
{



    uint32 MSNow = getMSTime();

    std::shared_ptr<DatabaseClient> pClient = this->GetClient();
    try
    {
        if(pClient != nullptr)
        {
            my_ulonglong res =  pClient->runCommand(command);
            pClient->State = Idle;
            this->TotalQueryTime += GetMSTimeDiffToNow(MSNow);
            this->TotalQuery++;
            return res;
        }
        else
        {
            sLog->LogMSG(MSG_ERROR,LTAG_DATABASE,"Failed To GetClient with command %s",command);//corectly later
        }
    }
    catch(MySqlException ex)
    {

        sLog->LogMSG(MSG_ERROR,LTAG_DATABASE,"Query : %s Error : %s ",command,ex.what());
        pClient->State = Broken;
    }




    return -1;

}

