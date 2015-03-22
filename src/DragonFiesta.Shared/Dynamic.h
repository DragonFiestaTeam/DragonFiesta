#ifndef DYNAMIC_H
#define DYNAMIC
class Dynamic
{
public:
    template<typename T>
    bool is()
    {
        return dynamic_cast<T*>(this) != nullptr;
    }
};

#endif
