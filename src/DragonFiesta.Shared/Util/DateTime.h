#ifndef DATETIME_H
#define DATETIME_H

#include <iostream>
#include <string>
#include <stdio.h>
#include <time.h>
#include "Define.h"

class DateTime
{
public:

    static std::string NowDate();
    static DateTime* Now();

    DateTime();
    DateTime(time_t Date);


    uint32 NowFiestaTime() const;

    //Main DateTimeStuff
    int32 MiliSeconds() const;
    int32 Second() const;
    int32 Minutes() const;
    int32 Hours() const;
    int32 Day() const;
    int32 Week() const;
    int32 Month() const;
    int32 Year() const;

    //Fiesta Stuff
    uint8 NowFiestaMonth() const;
    uint8 NowFiestaYear() const;

    uint32 ToFiestaTime(DateTime* pValue);
    uint32 ToFiestaMonth(DateTime* pValue);
    uint32 ToFiestaYear(DateTime* pValue);

    //DateTime Conversion Stuff
    uint32 ToInt();
    char* ToChar();
    // Math Stuff



    uint32 Subtract(DateTime* pValue);
    uint32 Subtract(DateTime* pValue1,DateTime* pValue2);

private:
    time_t Date;
    tm *LocalTime;



};

#endif
