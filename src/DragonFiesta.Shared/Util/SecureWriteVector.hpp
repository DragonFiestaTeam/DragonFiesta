#ifndef SECUREWRITEVECTOR_H
#define SECUREWRITEVECTOR_H

#include "../Define.h"
#include <vector>
#include <mutex>
#include <iostream>
#include <algorithm>



template<typename T>
class SecureWriteVector
{
public :

    bool Contains(const T& Value)
    {
        std::lock_guard<std::mutex> l(mMutex);
        auto result = std::find(std::begin(TVect), std::end(TVect), Value);
        return result != std::end(TVect);
    }

    template<typename Func>
    bool Contains(const Func& func)
    {
        std::lock_guard<std::mutex> l(mMutex);
        auto result = std::find_if(std::begin(TVect), std::end(TVect), func);
        return result != std::end(TVect);
    }
    //Find Value..

    bool Find(const T& Value,T& vOut)
    {
        std::lock_guard<std::mutex> l(mMutex);
        auto itr = std::find(std::begin(TVect), std::end(TVect), Value);
        if(itr != std::end(TVect))
        {
            vOut = *itr;
            return true;
        }


        return false;
    }
    //Find Value by Function
    template<typename func>
    bool Find(const func& _func,T& vOut)
    {
        std::lock_guard<std::mutex> l(mMutex);
        auto itr = std::find_if(std::begin(TVect), std::end(TVect),_func);
        if(itr != std::end(TVect))
        {
            vOut = *itr;
            return true;
        }

        return false;
    }

    template<typename Func>
    std::vector<T>  FindAll(const Func& func)
    {
        std::lock_guard<std::mutex> l(mMutex);
        std::vector<T> tmp;
        std::copy_if (TVect.begin(), TVect.end(), std::back_inserter(tmp), func );

        return tmp;
    }

    const int32&  capacity()
    {
        return TVect.capacity();
    }
    void Add(const T& Value)
    {
        std::lock_guard<std::mutex> l(mMutex);
        TVect.push_back(Value);
    }

    void clear()
    {
        TVect.clear();
    }

    size_t size()
    {
        return TVect.size();
    }

    bool empty()
    {
        return TVect.empty();
    }
    std::vector<T> get()
    {
        return TVect;
    }
    void print()
    {
        for(auto e : TVect)
        {
            std::cout << e << std::endl;
        }
    }

    void print(std::ostream& outStream)
    {
        std::lock_guard<std::mutex> l(mMutex);
        for(auto e : TVect)
        {
            outStream << e << std::endl;
        }
    }

    operator std::vector<T>() const
    {
        return TVect;
    }

    T operator [] (const int64& Pos)
    {
        return TVect[Pos];
    }
    T operator [](const std::string& Key)
    {
        return TVect[Key];
    }

protected :


    std::vector<T> TVect;
    std::mutex mMutex;

};

#endif
