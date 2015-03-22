#ifndef SECURECOLLECTOION_H
#define SECURECOLLECTOION_H


#include "SecureWriteCollection.hpp"
#include "../Define.h"

template<class T1,class T2>
class SecureWriteCollection;

//Key : Value
template<class T1,class T2>
class SecureCollection : public SecureWriteCollection<T1,T2>
{


public :


    bool Remove(T2 Value)
    {
        T1 tmpkey;
        if(FindKey(Value,tmpkey))
        {
            this->ObjectList.erase(tmpkey);
            return true;
        }
        return false;
    }

    bool Remove(T1 Key)
    {
        std::lock_guard<std::mutex> l(this->mMutex);
        auto res =   this->ObjectList.erase(Key);
        return res != std::end(this->ObjectList);
    }

    template<typename Func>
    void RemoveAll(Func func)
    {
        std::lock_guard<std::mutex> l(this->mMutex);
        auto _end = std::remove_if(this->ObjectList.begin(), this->ObjectList.end(), func);
        this->ObjectList.erase(_end, this->ObjectList.end());
    }

};

#endif
