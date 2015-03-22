#ifndef INTERPACKET_H
#define INTERPACKET_H

class InterPacket
{

public :
    std::size_t getId() const
    {
        return (std::size_t) mIdObject.get();
    }
private :

    std::shared_ptr<int32> mIdObject = sd::make_shared<int32>();
    virtual void Write();
    virtual bool Read();
};

#endif
