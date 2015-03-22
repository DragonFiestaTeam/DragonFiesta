#ifndef DATABASE_CLIENT_STATE
#define DATABASE_CLIENT_STATE


enum DatabaseClientState
{
    Open,
    Closed,
    Broken,
    Idle,
    Busy,
    None
};

#endif
