#ifndef LOGIN_QUERY_H
#define LOGIN_QUERY_H

enum LoginDataQuery : std::string
{
    SELECT_LoginVersions            = "SELECT * FROM Versions",

};

enum AccountQuerys : std::string
{
    INSERT_Account                  = "INSERT INTO Accounts (Name,password,Access_level) VALUES ('','','')",
    SELECT_AccountByName            = "SELECT ID,Name,password,Access_level,Banned,UnbanDate,IsOnline FROM accounts WHERE Name='{0}'",
    SELECT_AuthAccount              = "SELECT ID,Name,password,Access_level,Banned,UnbanDate,IsOnline FROM accounts WHERE Name='{0}' and password='{1}'",
    UPDATE_AccountAllOffline        = "UPDATE Accounts SET IsOnline='0' WHERE IsOnline='1'",
    UPDATE_AccountOnline            = "UPDATE Accounts SET IsOnline='{0}' WHERE ID='{1}'",
    UPDATE_BanState                 = "UPDATE Accounts SET Banned='{0}' WHERE ID='{1}'",
    UPDATE_TimeBanState             = "UPDATE Accounts SET Banned='{0}',banDate='{1}' WHERE ID='{2}'",

};

enum WorldQuerys : std::string
{

};

enum WorldCharacterQuerys : std::string
{
}

enum WorldGuildQuery : std::string
{
};

enum ZoneCharacterQuery : std::string
{
};

enum ZoneDataQuerys : std::string
{
};

enum ZoneItemQuery : std::string
{
};

enum ZoneGuildQuery : std::string
{
};

#endif
