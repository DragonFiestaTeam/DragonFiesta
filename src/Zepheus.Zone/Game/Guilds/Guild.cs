using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Fiesta.Zone.Game.Characters;
using Fiesta.Zone.Game.Guilds.Academy;
using Fiesta.Core.Cryptography;

namespace Fiesta.Zone.Game.Guilds
{
    public sealed class Guild
    {
        public int ID { get; private set; }
        public string Name { get; set; }

        public string Password
        {
            get
            {
                var data = _Password;
                InterCrypto.Decrypt(ref data, 0, data.Length);

                return Encoding.UTF8.GetString(data);
            }
            set
            {
                var data = Encoding.UTF8.GetBytes(value);
                InterCrypto.Encrypt(ref data, 0, data.Length);

                _Password = data;
            }
        }
        private byte[] _Password;

        public bool AllowGuildWar { get; set; }
        public string Message { get; set; }
        public int MessageCreaterID { get; set; }
        public DateTime MessageCreateTime { get; set; }
        public DateTime CreateTime { get; private set; }



        public List<GuildMember> Members { get; private set; }

        public GuildAcademy Academy { get; private set; }


        public object ThreadLocker { get; private set; }




        public Guild(SqlDataReader reader, SqlConnection con)
        {
            ID = reader.GetInt32(0);
            Name = reader.GetString(1);
            _Password = (byte[])reader.GetValue(2);

            AllowGuildWar = reader.GetBoolean(3);
            Message = reader.GetString(4);
            MessageCreateTime = reader.GetDateTime(5);
            MessageCreaterID = reader.GetInt32(6);
            CreateTime = reader.GetDateTime(7);


            Members = new List<GuildMember>();
            ThreadLocker = new object();

            Load(con);
        }
        public void Dispose()
        {
            Name = null;
            _Password = null;

            Message = null;


            Members.ForEach(m => m.Dispose());
            Members.Clear();
            Members = null;

            Academy.Dispose();
            Academy = null;

            ThreadLocker = null;
        }


        private void Load(SqlConnection con)
        {
            //members
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM GuildMembers WHERE GuildID = @pGuildID";

                cmd.Parameters.Add(new SqlParameter("@pGuildID", ID));


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var member = new GuildMember(this, reader);

                        Members.Add(member);
                    }
                }
            }

            //academy
            Academy = new GuildAcademy(this, con);
        }




        public bool GetMember(int CharacterID, out GuildMember Member)
        {
            lock (ThreadLocker)
            {
                Member = Members.Find(m => m.CharacterID.Equals(CharacterID));
            }

            return (Member != null);
        }
    }
}