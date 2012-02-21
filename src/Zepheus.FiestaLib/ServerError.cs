
namespace Zepheus.FiestaLib
{
    public enum ServerError : ushort
    {
        InvalidCredentials = 68,
        DatabaseError = 67,
        Exception = 65,
        Blocked = 71,
        ServerMaintenance = 72,
        Timeout = 73,
        AgreementMissing = 75,
        WrongRegion = 81,
    }
}
