USE `fiesta_world`;

-- Drop the existing table

DROP TABLE IF EXISTS
	`groups`;

-- Create new table
CREATE TABLE `groups` (
  `Id` bigint(20) NOT NULL DEFAULT '0',
  `Member1` int(10) unsigned DEFAULT NULL,
  `Member2` int(10) unsigned DEFAULT NULL,
  `Member3` int(10) unsigned DEFAULT NULL,
  `Member4` int(10) unsigned DEFAULT NULL,
  `Member5` int(10) unsigned DEFAULT NULL,
  `Exists` int(10) DEFAULT NULL,
  PRIMARY KEY (`Id`)
);

-- Rest any rest of groups in the characters table

UPDATE
	`characters`
SET
	`GroupID` = NULL,
	`IsGroupMaster` = NULL;

