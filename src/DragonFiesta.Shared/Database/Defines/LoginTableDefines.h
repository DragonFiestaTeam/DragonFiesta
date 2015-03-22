namespace DragonFiesta
{
// ID,Name,password,Access_level,Banned
typedef tuple<std::unique_ptr<uint64>, unique_ptr<string>, unique_ptr<int16>, unique_ptr<int32>> Account;
// VerHashm,Year,VerNumber
typedef tuple<unique_ptr<std::string>, unique_ptr<int16>, unique_ptr<int16>                      Version;


};
