#include "EntryDataProvider.h"


bool EntryDataProvider::GetQuestNPCInfo(const int32& ID,QuestNpcInfo& Out)
{
    if(QuestNpcByID.FindValue(ID,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::GetQuestNPCInfo(const std::string& InxName,QuestNpcInfo& Out)
{
    if(QuestNpcByInxName.FindValue(InxName,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::GetMobInfo(const int32& ID, MobInfo& Out)
{
    if(MobInfoByID.FindValue(ID,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::GetMobInfo(const std::string& InxName,MobInfo& Out)
{
    if(MobInfoByInxName.FindValue(InxName,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::GetNPCInfo(const int32& ID,NPCInfo& Out)
{
    if(NPCInfoByID.FindValue(ID,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::GetNPCInfo(const std::string& InxName,NPCInfo& Out)
{
    if(NPCInfoByInxName.FindValue(InxName,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::GetVendorInfo(const int32& ID,VendorInfo& Out)
{
    if(VendorInfoByID.FindValue(ID,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::GetVendorInfo(const std::string& InxName,VendorInfo& Out)
{
    if(VendorInfoByInxName.FindValue(InxName,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::GetSoulStoneVendorInfo(const int32& ID,SoulStoneVendorInfo& Out)
{
    if(SoulStoneVendorByID.FindValue(ID,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::GetSoulStoneVendorInfo(const std::string& InxName,SoulStoneVendorInfo& Out)
{
    if(SoulStoneVendorInfoByInxName.FindValue(InxName,Out))
    {
        return true;
    }
    return false;
}

bool EntryDataProvider::Load()
{
    return false;
}
void EntryDataProvider::LoadQuestNpcInfo()
{
}

void EntryDataProvider::LoadMobInfo()
{
}
void EntryDataProvider::LoadNPCInfo()
{
}
void EntryDataProvider::LoadVendorInfo()
{
}
void EntryDataProvider::LoadSouldStoneVendorInfo()
{

}



bool EntryDataProvider::Unload()
{
    return false;
}

