#ifndef DATABASE_MANAGER_H
#define DATABASE_MANAGER_H

#include "Task/Task.h"
#include "DatabaseClient.hpp"
#include "Database.h"
#include "DatabaseServer.h"
#include "Log/Log.h"
#include <map>
#include <mutex>
#include <list>
#include <memory>
#include <thread>
#include "Task/Timer.h"


class DatabaseManager : public Task
{

//void RunSQL(std::map<std::shared_ptr<std::string>> QueryList);
public:

    //Task
    void OnLeave() override;
    void OnAdded() override;

    DatabaseManager(uint32 intervall);
    DatabaseManager(std::shared_ptr<Database> DB,std::shared_ptr<DatabaseServer> DBServer);

    template <typename... InputArgs, typename... OutputArgs>
    void RunQuery(std::vector<std::tuple<OutputArgs...>>* const results, const char* const query, const InputArgs&... args);




    template <typename... Args>
    my_ulonglong runCommand(const char* const command, const Args&... args);
    my_ulonglong runCommand(const char* const command);

    template <typename... Args>
    my_ulonglong runCommand(const MySqlPreparedStatement& statement, const Args&... args);
    my_ulonglong runCommand(const MySqlPreparedStatement& statement);



    inline uint64 GetTotalQuerys() { return TotalQuery; }
    inline uint32 GetTotalQueryTime()  {return TotalQueryTime; }
    inline void ResetTotalCounters() { TotalQuery = 0; TotalQueryTime = 0; }

    void StopMonitoring();
private :


    void UpdateClients();
    void DestroyClients();
    void ClientDisconnecting(uint32 HandleID);
    void ClientConnecting(uint32 HandleID);
    void PrintMysqlInfos();

    uint32                            GetConnectionCount();
    std::shared_ptr<DatabaseClient>   GetClient();




    std::vector<std::shared_ptr<DatabaseClient>> mClients;
    std::mutex mMutex;

    std::shared_ptr<Database> mDatabase;
    std::shared_ptr<DatabaseServer> mServer;
    uint32 TotalQueryTime = 0;
    uint64 TotalQuery = 0;



};

template <typename... Args>
my_ulonglong DatabaseManager::runCommand(const char* const command,const Args&... args)
{

    uint32 MSNow = getMSTime();
    std::shared_ptr<DatabaseClient> pClient = this->GetClient();
    try
    {
        if(pClient != nullptr)
        {
            my_ulonglong res =  pClient->runCommand(command,args...);
            pClient->State = Idle;
            this->TotalQueryTime += GetMSTimeDiffToNow(MSNow);
            this->TotalQuery++;
            return res;
        }
        else
        {
            sLog->LogMSG(MSG_ERROR,LTAG_DATABASE,"Failed To GetClient for command %s",command);//corectly later
        }
    }
    catch(MySqlException ex)
    {
        sLog->LogMSG(MSG_ERROR,LTAG_DATABASE,"Query : %s Error : %s ",command,ex.what());
        pClient->State = Broken;
    }

    return -1;
}



template <typename... Args>
my_ulonglong DatabaseManager::runCommand(const MySqlPreparedStatement& statement,const Args&... args)
{

    uint32 MSNow = getMSTime();
    std::shared_ptr<DatabaseClient> pClient = this->GetClient();
    try
    {
        if(pClient != nullptr)
        {
            my_ulonglong res =  pClient->runCommand(statement,args...);
            pClient->State = Idle;
            this->TotalQueryTime += GetMSTimeDiffToNow(MSNow);
            this->TotalQuery++;
            return res;
        }
        else
        {
            sLog->LogMSG(MSG_ERROR,LTAG_DATABASE,"Failed To GetClient with prepared Statement");//corectly later
        }
    }
    catch(MySqlException ex)
    {
        pClient->State = Broken;
        //Log(ERROR,DATABASE,"Query : %s Error %s ",command,ex.what());
    }



    return -1;
}


template <typename... InputArgs, typename... OutputArgs>
void DatabaseManager::RunQuery(std::vector<std::tuple<OutputArgs...>>* const results, const char* const query, const InputArgs&... args)
{
    uint32 MSNow = getMSTime();
    std::shared_ptr<DatabaseClient> pClient = this->GetClient();
    try
    {
        if(pClient != nullptr)
        {
            pClient->runQuery(results,query,args...);
            pClient->State = Idle;
            this->TotalQueryTime += GetMSTimeDiffToNow(MSNow);
            this->TotalQuery++;
        }
        else
        {
            sLog->LogMSG(MSG_ERROR,LTAG_DATABASE,"Failed To GetClient with query %s",query);
        }
    }
    catch(MySqlException ex)
    {
        sLog->LogMSG(MSG_ERROR,LTAG_DATABASE,"Query : %s Error : %s ",query,ex.what());

        pClient->State = Broken;
    }


}

#endif
