ALTER TABLE characters ADD COLUMN  `GroupID` int(11) unsigned NOT NULL DEFAULT '0';
ALTER TABLE characters ADD COLUMN  `IsGroupMaster` tinyint(1) unsigned NOT NULL DEFAULT '0';