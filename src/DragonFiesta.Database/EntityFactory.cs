using DragonFiesta.Database.EF_Models;
using DragonFiesta.Database.DatabaseUtil;

namespace DragonFiesta.Database
{
    public class EntityFactory
    {

        public static AccountEntities GetAccountEntity(EntitySetting setting)
        {
            string connectionstring = ConnectionStringbuilder.CreateEntityString(setting);
            return new AccountEntities(connectionstring);
        }
        public static string GetAccountEntityConnectionString(EntitySetting setting)
        {
            return ConnectionStringbuilder.CreateEntityString(setting);
        }
        //here more connection stuff zone & world
    }
}
