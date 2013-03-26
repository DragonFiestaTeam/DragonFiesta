using System;
using DragonFiesta.Networking;
using DragonFiesta.Networking.Handler.Client;
using DragonFiesta.Util;
using DragonFiesta.FiestaLib;
using DragonFiesta.World;
using DragonFiesta.World.Database;
using DragonFiesta.World.Networking.ServerHandler;
using DragonFiesta.World.Game;
using DragonFiesta.Data;

namespace DragonFiesta.World.Networking.ClientHandler
{
    [PacketHandlerClass(CH5Type._Header)]
    public static class CH5Methods
    {
        [PacketHandler(CH5Type.CreateCharacter)]
        public static void CreateCharacter(WorldClient sender, Packet packet)
        {
            string name;
            byte slot, jobGender, hair, color, style;
            if (!packet.TryReadByte(out slot) || !packet.TryReadString(out name, 16) ||
                !packet.TryReadByte(out jobGender) || !packet.TryReadByte(out hair) ||
                !packet.TryReadByte(out color) || !packet.TryReadByte(out style))
            {
                Console.WriteLine("Error reading create char for {0}");
                return;
            }
            if (DragonFiesta.World.Database.CharacterDB.IsNameInUse(name))
            {
                SH5Methods.SendCharCreationError(sender, CreateCharError.NameInUse);
                return;
            }
            if (sender.CharacterList.Count >= 5)
            {
                SH5Methods.SendCharCreationError(sender, CreateCharError.ErrorInMaxSlot);
                return;
            }
            else
            {

                byte isMaleByte = (byte)((jobGender >> 7) & 0x01);
                byte classIDByte = (byte)((jobGender >> 2) & 0x1F);

                WorldCharacter pChar = new WorldCharacter
                {
                    AccountInfo = sender.AccountInfo,
                    CharacterLevel = 1,
                    CharacterSlot = DragonFiesta.World.Database.CharacterDB.GenerateCharacterSlot(sender),
                    Name = name,
             
                    CharacterID = DragonFiesta.World.Database.CharacterDB.GenerateCharacterID(),
                };

                CharacterClass pClass = new CharacterClass
                {
                    pClassID = (ClassID)classIDByte,
                };

                pChar.Look = new LookInfo
                {
                    Hair = hair,
                    HairColor = color,
                    Male = isMaleByte,
                    Face = style,
                };

                pChar.Position = new PositionInfo
                {
                    Map = 0,
                    XPos = 7636,
                    YPos = 4610,
                };
                pChar.Create();
                sender.CharacterList.Add(pChar.CharacterSlot, pChar);
                pChar.SendCreate(sender);
            }
        }

        [PacketHandler(CH5Type.DeleteCharacter)]
        public static void DeleteCharacterHandler(WorldClient client, Packet packet)
        {
             byte slot;
             if (!packet.TryReadByte(out slot) || slot > 10 || client.CharacterList[slot] == null)
             {
             }
             client.CharacterList[slot].SendDelete(client,slot);
             client.CharacterList[slot].Delete();
             client.CharacterList[slot] = null;
        }
    }
}
