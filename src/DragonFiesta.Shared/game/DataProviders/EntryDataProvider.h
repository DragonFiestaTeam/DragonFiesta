#ifndef ENTRYDATAPROVIDER_H
#define ENTRYDATAPROVIDER_H

#include "../../Util/Singleton.h"
#include "../../Define.h"
#include "../../Util/SecureCollection.hpp"


#include "../Data/VendorInfo.h"
#include "../Data/MobInfo.h"
#include "../Data/NPCInfo.h"
#include "../Data/QuestNpcInfo.h"
#include "../Data/SoulStoneVendorInfo.h"
#include "../Data/GateInfo.h"

class EntryDataProvider
{

public :

    bool GetQuestNPCInfo(const int32& ID,QuestNpcInfo& Out);
    bool GetQuestNPCInfo(const std::string& InxName,QuestNpcInfo& Out);
    bool GetMobInfo(const int32& ID, MobInfo& Out);
    bool GetMobInfo(const std::string& InxName,MobInfo& Out);
    bool GetNPCInfo(const int32& ID,NPCInfo& Out);
    bool GetNPCInfo(const std::string& InxName,NPCInfo& Out);
    bool GetVendorInfo(const int32& ID,VendorInfo& Out);
    bool GetVendorInfo(const std::string& InxName,VendorInfo& Out);
    bool GetSoulStoneVendorInfo(const int32& ID,SoulStoneVendorInfo& Out);
    bool GetSoulStoneVendorInfo(const std::string& InxName,SoulStoneVendorInfo& Out);
    bool Load();
    bool Unload();

private:


    bool IsInitialized;


    SecureWriteCollection<const int32,QuestNpcInfo>                    QuestNpcByID;
    SecureWriteCollection<const int32,MobInfo>                         MobInfoByID;
    SecureWriteCollection<const int32,NPCInfo>                         NPCInfoByID;
    SecureWriteCollection<const int32,VendorInfo>                      VendorInfoByID;
    SecureWriteCollection<const int32,SoulStoneVendorInfo>             SoulStoneVendorByID;
    SecureWriteCollection<const int32,GateInfo>                        GateInfoByID;

    SecureWriteCollection<const std::string,QuestNpcInfo>              QuestNpcByInxName;
    SecureWriteCollection<const std::string,MobInfo>                   MobInfoByInxName;
    SecureWriteCollection<const std::string,NPCInfo>                   NPCInfoByInxName;
    SecureWriteCollection<const std::string,VendorInfo>                VendorInfoByInxName;
    SecureWriteCollection<const std::string,SoulStoneVendorInfo>       SoulStoneVendorInfoByInxName;
    SecureWriteCollection<const std::string,GateInfo>                  GateInfoByInxName;


    void LoadQuestNpcInfo();
    void LoadMobInfo();
    void LoadNPCInfo();
    void LoadVendorInfo();
    void LoadSouldStoneVendorInfo();

#define sEntryProver EntryDataProvider::GetInstance()
};

#endif
