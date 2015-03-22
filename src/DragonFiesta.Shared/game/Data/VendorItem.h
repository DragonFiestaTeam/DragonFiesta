#ifndef VENDORITEM_H
#define VENDORITEM_H

#include "ItemInfo.h"


class VendorItem
{
public :
    ItemInfo GetItem()
    {
        return NULL;//here get ItemInfo From ItemDataProvider
    }
    int32 GetCount()
    {
        return ItemCount;
    }

private :
    ItemID;
    ItemCount;


};

#endif
