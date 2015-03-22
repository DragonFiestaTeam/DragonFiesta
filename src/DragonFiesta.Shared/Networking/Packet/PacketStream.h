#ifndef _PACKETSTREAM
#define _PACKETSTREAM

#include "Define.h"
#include <exception>
#include <cassert>
#include <list>
#include <map>
#include <string>
#include <vector>
#include <cstring>
#include <time.h>
#include <cmath>
#include <type_traits>
#include <boost/asio/buffer.hpp>

#include "PacketExecption.h"


class PacketStream
{

public :


    inline void resize(size_t newsize)
    {
        _storage.resize(newsize, 0);
        StreamPos = 0;
    }

    inline void reserve(size_t ressize)
    {
        if (ressize > size())
            _storage.reserve(ressize);
    }

    inline void clear()
    {
        StreamPos = 0;
        _storage.clear();
    }

    void print_storage() const;

    void textlike() const;

    void hexlike() const;

    inline uint8* contents()
    {
        if (_storage.empty())
            throw PacketStreamException();
        return _storage.data();
    }

    inline uint8 const* contents() const
    {
        if (_storage.empty())
            throw PacketStreamException();
        return _storage.data();
    }


    inline uint8& operator[](size_t const pos)
    {
        if (pos >= size())
            throw PacketStreamPositionException(false, pos, 1, size());
        return _storage[pos];
    }

    inline uint8 const& operator[](size_t const pos) const
    {
        if (pos >= size())
            throw PacketStreamPositionException(false, pos, 1, size());
        return _storage[pos];
    }


    inline size_t GetPos() const
    {
        return StreamPos;
    }
    size_t size() const
    {
        return _storage.size();
    }
    bool   empty() const
    {
        return _storage.empty();
    }

    static size_t const DEFAULT_SIZE = 0x1000;

protected:
    size_t StreamPos;
    std::vector<uint8> _storage;
};


namespace boost
{
namespace asio
{
inline const_buffers_1 buffer(PacketStream const& _Stream)
{
    return buffer(_Stream.contents(), _Stream.size());
}
}
}


#endif
