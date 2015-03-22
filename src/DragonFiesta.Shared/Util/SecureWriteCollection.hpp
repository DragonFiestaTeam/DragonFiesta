#ifndef SECUREWRITECOLLECTOION_H
#define SECUREWRITECOLLECTOION_H

#include <mutex>
#include <map>
#include <utility>
#include <iostream>
#include <algorithm>

//Key : Value
template<class T1, class T2>
class SecureWriteCollection
{

public:

    SecureWriteCollection()
    {

    }

    bool ContainsValue(const T2& Value)
    {
        return  ContainsValue(Value,[&Value](const T1& v)
        {
            return v.second == Value;
        });
    }
    bool ContainsKey(const T1& key)
    {
        std::lock_guard<std::recursive_mutex> l(mMutex);
        auto result = ObjectList.find(key);
        return result != std::end(ObjectList);
    }

    template<typename Func>
    bool ContainsKey(const Func& func)
    {
        std::lock_guard<std::recursive_mutex> l(mMutex);
        auto result  = std::find_if(std::begin(ObjectList), std::end(ObjectList),func);
        return result != std::end(ObjectList);
    }

    template<typename Func>
    bool ContainsValue(const Func& _func)
    {
        std::lock_guard<std::recursive_mutex> l(mMutex);
        auto result = std::find_if(std::begin(ObjectList),std::endl((ObjectList),_func));
        return result != std::end(ObjectList);
    }
    template<typename Func>
    std::map<T1,T2> FindAll(const Func& func)
    {
        std::lock_guard<std::recursive_mutex> l(mMutex);
        std::map<T1,T2> tmp;
        std::copy_if (ObjectList.begin(), ObjectList.end(), std::back_inserter(tmp), func );

        return tmp;

    }
    template<typename Func>
    bool FindValue(const Func& _func,T2& outvalue)
    {
        std::lock_guard<std::recursive_mutex> l(mMutex);
        auto res = std::find_if(std::begin(ObjectList), std::end(ObjectList),_func);
        if(res != std::end(ObjectList))
        {
            outvalue = *res.second;
            return true;
        }
        return false;
    }

    bool FindValue(const T1& key,T2& outValue)
    {
        std::lock_guard<std::recursive_mutex> l(mMutex);
        auto result = ObjectList.find(key);
        if(result != std::end(ObjectList))
        {
            outValue = result->second;
            return true;
        }
        return false;
    }

    bool FindKey(const T2& Value,T1& outKey)
    {
        std::lock_guard<std::recursive_mutex> l(mMutex);
        auto res = std::find_if(std::begin(ObjectList), std::end(ObjectList),[Value] (const T1& v)
        {
            return Value == v.second;
        });

        if(res != std::end(ObjectList))
        {
            outKey = *res;
            return true;
        }
        return false;
    }

    template<typename Func>
    bool FindKey(const Func& func,T1& outkey)
    {
        std::lock_guard<std::recursive_mutex> l(mMutex);
        auto res = std::find_if(std::begin(ObjectList), std::end(ObjectList),func);
        if( res != std::end(ObjectList))
        {
            outkey = *res;
            return true;
        }
        return false;
    }

    bool TryAdd(const T1& Key,const T2& Value)
    {
        std::lock_guard<std::recursive_mutex> l(mMutex);
        if(!ContainsKey(Key))
        {
            ObjectList.insert (std::pair<T1,T2>(Key,Value));
            return true;
        }

        return false;
    }

    void clear()
    {
        ObjectList.clear();
    }

    size_t size()
    {
        return ObjectList.size();
    }

    bool empty()
    {
        return ObjectList.empty();
    }
    const std::map<T1, T2>& get()
    {
        return ObjectList;
    }
    void print()
    {
        std::lock_guard<std::recursive_mutex> l(this->mMutex);
        for (auto t : ObjectList)
        {
            std::cout << t.first << " "
                      << t.second << " "
                      << std::endl;
        }
    }

    void print(std::ostream& outStream)
    {
        std::lock_guard<std::recursive_mutex> l(this->mMutex);
        for(auto t : ObjectList)
        {
            outStream << t.first << " "
                      << t.second << " "
                      << std::endl;
        }
    }

    operator std::map<T1,T2>() const
    {
        return ObjectList;
    }

    T2 operator [] (T1 key)
    {
        return ObjectList[key];
    }

protected :

    std::map<T1, T2>  ObjectList;
    std::recursive_mutex mMutex;
};

#endif
