using System;
using Zepheus.World.Data;


namespace Zepheus.World.Events
{
    public class OnCharacterLevelUpArgs : EventArgs
    {
        public WorldCharacter PCharacter { get; set; }

        public OnCharacterLevelUpArgs(WorldCharacter pChar)
        {
            this.PCharacter = pChar;
        }
        public OnCharacterLevelUpArgs()
        {
        }
    }
}
