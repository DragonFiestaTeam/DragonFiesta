#include "FiestaCryptoProvider.h"

#include <stdexcept>


FiestaCryptoProvider::FiestaCryptoProvider(const int16& offset)
{
    if (offset >= 499) throw std::runtime_error("Xor offset cannot be bigger than 499.");
    XorPos = offset;
}

void FiestaCryptoProvider::Crypt(std::vector<unsigned char>& byteBuffer) {
    Crypt(byteBuffer, 0, byteBuffer.size());
}

void FiestaCryptoProvider::Crypt(std::vector<unsigned char>& byteBuffer, const int32& pOffset, const int32& pLen) {


    for (int i = 0; i < pLen; ++i)
    {
        byteBuffer[pOffset + i] ^= XorTable[XorPos];
        ++XorPos;
        XorPos %= 499;
    }

}
