#ifndef SECUREVECTORCOLLECTION_H
#define SECUREVECTORCOLLECTION_H

#include <vector>
#include <mutex>
#include <iostream>
#include "../Define.h"
#include <algorithm>


#include "Util/SecureWriteVector.hpp"



//Fucking gcc bug -.-
//using this->mMutex etc to fix
//c++ Standart say mMutex
//fucking shit of compiler bugs -.-


template<typename T>
class SecureVectorCollection : public SecureWriteVector<T>
{
//whats the fuck why must be use this
public :

    bool Remove(T Value)
    {
        std::lock_guard<std::mutex> l(this->mMutex);
        auto res =   this->TVect.erase(Value);

        return res != std::end(this->TVect);
    }
    template<typename Func>
    void RemoveAllBy(Func func)
    {
        std::lock_guard<std::mutex> l(this->mMutex);
        auto _end = std::remove_if(this->TVect.begin(), this->TVect.end(), func);
        this->TVect.erase(_end, this->TVect.end());
    }
    void RemoveAllBy(T value)
    {
        RemoveAllBy([value](const T& v)
        {
            return v == value;
        });
    }

};


#endif // SecureCollectionVector_H
