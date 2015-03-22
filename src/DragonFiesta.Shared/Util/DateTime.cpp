#include "DateTime.h"



//TODO : USE BOOST

std::string DateTime::NowDate()
{
    time_t     now = time(0);
    struct tm  tstruct;
    char       buf[80];
    tstruct = *localtime(&now);

    strftime(buf, sizeof(buf), "%Y-%m-%d.%X", &tstruct);

    return buf;
}

DateTime::DateTime()
{
    this->Date = time(0);
    this->LocalTime = localtime(&Date);
}

DateTime::DateTime(time_t Date)
{
    this->Date = Date;
    this->LocalTime = localtime(&Date);
}

//Constructors end


DateTime* DateTime::Now()
{

    DateTime* DT = new DateTime();
    return DT;
}

uint32 DateTime::NowFiestaTime() const
{
    uint32 val = 0;

    val |= (uint32)(LocalTime->tm_sec << 25);
    val |= (uint32)((LocalTime->tm_hour & 0x3F) << 19);
    val |= (uint32)((LocalTime->tm_mday & 0x3F) << 13);
    val |= (uint32)((LocalTime->tm_mon & 0x1F) << 8);
    val |= (char)(LocalTime->tm_year - 2000);

    return val;
}

uint8 DateTime::NowFiestaMonth() const
{
    return (LocalTime->tm_mon << 4);
}

uint8 DateTime::NowFiestaYear() const
{
    return LocalTime->tm_year -1900;
}

int32 DateTime::Second() const
{
    return LocalTime->tm_sec;
}

int32 DateTime::Minutes() const
{
    return LocalTime->tm_min;
}

int32 DateTime::Hours() const
{
    return LocalTime->tm_hour;
}

int32 DateTime::Day() const
{
    return LocalTime->tm_mday;
}

int32 DateTime::Week() const
{
    return LocalTime->tm_wday;
}

int32 DateTime::Month() const
{
    return LocalTime->tm_mon+1;
}

int32 DateTime::Year() const
{
    return LocalTime->tm_year+1900;
}



uint32 DateTime::ToFiestaTime(DateTime* pValue)
{

    uint32 val = 0;

    val |= (uint32)(pValue->Second() << 25);
    val |= (uint32)((pValue->Hours() & 0x3F) << 19);
    val |= (uint32)((pValue->Day() & 0x3F) << 13);
    val |= (uint32)((pValue->Month() & 0x1F) << 8);
    val |= (char)(pValue->Year() - 2000);

    return val;

}

uint32 DateTime::ToFiestaMonth(DateTime* pValue)
{
    return (pValue->Month() << 4);
}

uint32 DateTime::ToFiestaYear(DateTime* pValue)
{
    return LocalTime->tm_year -1900;
}

uint32 DateTime::ToInt()
{
    return this->Date;
}

char* DateTime::ToChar()
{
    return ctime(&Date);
}


uint32 DateTime::Subtract(DateTime* pValue)
{
    return this->ToInt() - pValue->ToInt();
}

uint32 DateTime::Subtract(DateTime* pValue,DateTime* pValue2)
{
    return pValue->ToInt() - pValue2->ToInt();
}
