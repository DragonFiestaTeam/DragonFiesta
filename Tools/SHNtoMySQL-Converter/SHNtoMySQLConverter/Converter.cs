using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SHNtoMySQLConverter.SHN;
using SHNtoMySQLConverter;
using System.IO;

namespace SHNtoMySQLConverter
{
    class Converter
    {
        public string getTables(SHNFile file)
        {
            string tables = "";
            for (int j = 0; j < file.ColumnCount; ++j)
            {
                tables += (((SHNColumn)file.Columns[j]).ColumnName);
                if (j < file.ColumnCount - 1) tables += ",";
            }

            return FormatBuilder(tables);
        }
        /*
        DROP TABLE IF EXISTS `data_iteminfo`;
        CREATE TABLE `data_iteminfo` (`ID` smallint(5) unsigned NOT NULL,next,  PRIMARY KEY (`ID`),) ENGINE=InnoDB DEFAULT CHARSET=utf8;
        */
        public bool MakeTable(SHNFile file, string newtable, bool nokey)
        {
            //types = "'"+file.Columns[x].ColumnName+"' int(11) unsigned NOT NULL, TypeByte: "+((SHNColumn)file.Columns[x]).TypeByte;
            /* short = smallint
             * ushort = smallint UNSIGNED
             * sbyte = tinyint
             * byte = tinyint UNSIGNED
             * int = int
             * uint = int UNSIGNED
             * long = bigint
             * ulong = bigint unsigned
             * float = float
             * double = double
             * decimal = decimal

             * Int16 -> short
             * Int32 -> int  
             * Int64 -> long 
*/

            string types = "CREATE TABLE " + newtable + " (";
            for (int x = 0; x < file.ColumnCount; ++x)
            {
                types += FormatBuilder(file.Columns[x].ColumnName) + ((SHNColumn)file.Columns[x]).MYSQLType;
                if (x < file.ColumnCount - 1) types += ",";
                //                types += (((SHNColumn)file.Columns[x]).GetType())+" - ";
                //if (x < file.ColumnCount-1) tables += ",";
                //Console.WriteLine(file.Columns[x].ColumnName.Replace(" ", "") + ((SHNColumn)file.Columns[x]).MYSQLType + "TypeByte: " + ((SHNColumn)file.Columns[x]).TypeByte);
            }
            if (((SHNColumn)file.Columns[0]).TypeByte != 24 && ((SHNColumn)file.Columns[0]).TypeByte != 26 && ((SHNColumn)file.Columns[0]).TypeByte != 9 && file.FileName != "QuestDialog.shn" && file.FileName != "BasicInfoLink.shn" && (!nokey)) types += ",PRIMARY KEY (" + FormatBuilder(file.Columns[0].ColumnName) + ")";
            types += ")ENGINE=InnoDB DEFAULT CHARSET=utf8;";
            string text = "Dropping/Building table : [" + newtable + "] ...";
            Console.Write(text);
            Database.DatabaseHelper.Instance.runSQL("DROP TABLE IF EXISTS " + newtable + "");
            //Log.Write(LogLevel.Info, types);
            if (Database.DatabaseHelper.Instance.runSQL(types)) Console.WriteLine("\r" + text + " done"); else { Log.Writer.Write(" failed"); return false; }


            return true;
        }

        public string FormatBuilder(string text)
        {
            return text.Replace(" ", "")
                        .Replace("First", "First_")
                        .Replace("Range", "Range_")
                        .Replace("INDEX", "INDEX_")
                        .Replace("Index", "Index_")
                        .Replace("INT", "INT_")
                        .Replace("Desc", "Description")
                        .Replace("Text", "Text_")
                        .Replace("From", "From_")
                        .Replace("To", "To_");
        }
        public string getData(SHNFile file, int index)
        {
            object[] data = new object[file.Rows.Count + 2];
            file.Rows.CopyTo(data, 1);
            string daten = "'";
            int z = 0;
            DataRow row = file.Rows[index];
            foreach (object item in row.ItemArray)
            {
                z++;
                daten += ConvertToUTF8("" + item);
                if (z < file.ColumnCount) daten += "','"; else daten += "'";
            }

            return daten.Replace("-", "");
        }

        public string ConvertToUTF8(string text)
        {
            var utf8String = Encoding.UTF8.GetBytes(text);
            return Encoding.UTF8.GetString(utf8String).Replace("'", "");
        }

        public bool SendtoDB(SHNFile file, int inx, string table)
        {
            //Log.WriteLine(LogLevel.Info, "VALUES (" + getData(file, inx) + ")");
            if (Database.DatabaseHelper.Instance.runSQL("INSERT INTO " + table + " (" + getTables(file) + ") VALUES (" + getData(file, inx) + ")"))
            {
                return true;
            }
            return false;
        }

        public void Convert(string filename, string table, bool nokey)
        {

            if (!File.Exists(filename)) { Console.WriteLine("{0} doesn't exists ... skipping!", filename); return; }
            SHNFile file = new SHNFile(filename);
            int count = 0;
            MakeTable(file, table, nokey);
            

            for (int inx = 0; inx < file.Rows.Count; inx++)
            {
                if (SendtoDB(file, inx, table)) count++;
                Console.Write("\rImporting [{3}] to Database : {0}% [{1}/{2}]", (int)((inx + 1) / (double)file.Rows.Count * 100), inx + 1, file.Rows.Count, file.FileName);
                
                if (count == file.Rows.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\rImporting [{3}] to Database : {0}% [{1}/{2}] ... done", (int)((inx + 1) / (double)file.Rows.Count * 100), inx + 1, file.Rows.Count, file.FileName);
                    Console.ForegroundColor = ConsoleColor.White;                    
                }
                else if (inx == file.Rows.Count && count != file.Rows.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\rImporting [{3}] to Database : {0}% [{1}/{2}] ... failed", (int)((inx + 1) / (double)file.Rows.Count * 100), inx + 1, file.Rows.Count, file.FileName);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            }
        }

        public void MainConvert()
        {
            Log.SetLogToFile(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "//Log/DBError.txt");

                                    Convert("AbState.shn", "data_abstate",false);                                    
                                    Convert("AbStateView.shn", "data_abstateview",false);
                                    Convert("ActionEffectInfo.shn", "data_actioneffectinfo",false);
                                    Convert("ActiveSkill.shn", "data_activeskill",true);
                                    Convert("ActiveSkillGroup.shn", "data_activeskillgroup",false); //Leer ?!
                                    Convert("ActiveSkillView.shn", "data_activeskillview",false);
                                   Convert("AttendReward.shn", "data_attendreward",false);
                                    Convert("BadNameFilter.shn", "data_badnamefilter",false);
                                    Convert("BasicInfoHelp.shn", "data_basicinfohelp",false);
                                    Convert("BasicInfoTip.shn", "data_basicinfotip",false);
                                    Convert("BasicInfoLink.shn", "data_basicinfolink",false);
                                    Convert("BasicInfoTipCycle.shn", "data_basicinfotipcycle",false);
                                    Convert("BasicInfoTitle.shn", "data_basicinfotitle",false);
                                    Convert("BelongTypeInfo.shn", "data_belongtypeinfo",false);
                                    Convert("CharacterTitleData.shn", "data_charactertitle",false);
                                    Convert("CharacterTitleStateView.shn", "data_charactertitlestateview",true);
                                    Convert("ChargedEffect.shn", "data_chargedeffect",false);
                                    Convert("ChargedIconItem.shn", "data_chargedIconItem",false);
                                    Convert("ChargedMessageItem.shn", "data_chargedmessageitem",false);
                                    Convert("ClassName.shn", "data_classname",false);
                                    Convert("ColorInfo.shn", "data_colorinfo",false);
                                    Convert("DamageEffect.shn", "data_damageeffect",false);
                                    Convert("DamageSoundInfo.shn", "data_damagesoundinfo",false);
                                    Convert("DiceDividind.shn", "data_dicedividind",false);
                                    Convert("EffectViewInfo.shn", "data_effectviewinfo",false);
                                    Convert("EmotionFilter.shn", "data_emotionfilter",false);
                                    Convert("ErrorCodeTable.shn", "data_errorcodetable",false);
                                    Convert("FaceInfo.shn", "data_faceinfo",false);
                                    Convert("Gather.shn", "data_gather",true);
                                    Convert("GBDiceDividind.shn", "data_gbdicedividind",false);
                                    Convert("GBHouse.shn", "data_gbhouse",false);
                                    Convert("GradeItemOption.shn", "data_gradeitemoption",false);
                                    Convert("GTIView.shn", "data_gtiview",false);
                                    Convert("GuildGradeData.shn", "data_guildgrade",true);
                                    Convert("GuildTournamentRequire.shn", "data_guildtournamentrequire",false);
                                    Convert("GuildTournamentSkill.shn", "data_guildtournamentskill",true);
                                    Convert("GuildTournamentSkillDesc.shn", "data_guildtournamentskilldesc",true);
                                    Convert("HairColorInfo.shn", "data_haircolorinfo",false);
                                    Convert("HairInfo.shn", "data_hairinfo",false);
                                    Convert("ItemDismantle.shn", "data_itemdismantle",false);
                                    Convert("ItemInfo.shn", "data_iteminfo",false);
                                    Convert("ItemInfoServer.shn", "data_iteminfoserver",false);
                                    Convert("ItemShop.shn", "data_itemshop",true);
                                    Convert("ItemShopView.shn", "data_itemshopview",true);
                                    Convert("ItemUseEffect.shn", "data_itemuseeffect",false);
                                    Convert("ItemViewEquipeTypeInfo.shn", "data_itemviewequiptypeinfo",false);
                                    Convert("itemviewinfo.shn", "data_itemviewinfo",false);
                                    //KingDomQuestDesc.shn - not necessary - skipping
                                    Convert("MapInfo.shn", "data_mapinfo",false);
                                    Convert("MapLinkPoint.shn", "data_maplinkpoint",true);
                                    Convert("MapViewInfo.shn", "data_mapviewinfo",false);
                                    Convert("MapWayPoint.shn", "data_mapwaypoint",true);
                                    Convert("MHEmotion.shn", "data_mhemotion",true);
                                    Convert("MiniHouse.shn", "data_minihouse",false);
                                    Convert("MiniHouseDummy.shn", "data_minihousedummy",false);
                                    Convert("MiniHouseEndure.shn", "data_minihouseendure",false);
                                    Convert("MiniHouseFurniture.shn", "data_minihousefurniture",false);
                                    Convert("MiniHouseFurnitureObjEffect.shn", "data_minihousefurnitureobjeffect",false);
                                    Convert("MiniHouseObjAni.shn", "data_minihouseobjani",false);
                                    Convert("MobCoordinate.shn", "data_mobcoordinate",false);
                                    Convert("MobInfo.shn", "data_mobinfo",false);
                                    Convert("MobKillAnnounceText.shn", "data_mobkillannouncetext",false);
                                    Convert("MobRandomIdleAni.shn", "data_mobrandomidleani",false);
                                    Convert("MobSpecies.shn", "data_mobspecies",false);
                                    Convert("MobViewInfo.shn", "data_mobviewinfo",false);
                                    Convert("NPCViewInfo.shn", "data_npcviewinfo",false);
                                    Convert("PassiveSkill.shn", "data_passiveskill",false);
                                    Convert("PassiveSkillView.shn", "data_pasiveskillview",false);
                                    Convert("Produce.shn", "data_produce",false);
                                    Convert("ProduceView.shn", "data_produceview",false);
                                    // QuestData.shn is no SHN - it's a script - skipping
                                    Convert("QuestDialog.shn", "data_questdialog",false);
                                    Convert("RaceNameInfo.shn", "data_racenameinfo",false);
                                    Convert("Riding.shn", "data_riding",false);
                                    Convert("ScriptMsg.shn", "data_scriptmsg",false);
                                    Convert("SetItem.shn", "data_setitem",false);
                                    Convert("SetItemEffect.shn", "data_setitemeffect",false);
                                    Convert("SetItemView.shn", "data_setitemview",false);
                                    Convert("SlanderFilter.shn", "data_slanderfilter",false);
                                    Convert("SubAbState.shn", "data_subabstate",true);
                                    Convert("TextData.shn", "data_textdata",false);
                                    Convert("TextData2.shn", "data_textdata2",false);
                                    Convert("TownPortal.shn", "data_townportal",true);
                                    Convert("UpEffect.shn", "data_upeffect",false);
                                    Convert("UpgradeInfo.shn", "data_upgradeinfo",false);
                                    Convert("WeaponAttrib.shn", "data_weaponattrib",false);
                                    Convert("WeaponTitleData.shn", "data_weapontitle",true);
                                    Convert("WorldMapAvatarInfo.shn", "data_worldmapavatarinfo",false);

        } 
    }
}
