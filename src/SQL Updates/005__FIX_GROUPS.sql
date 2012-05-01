USE `fiesta_world`;

-- Drop the existing table

DROP TABLE IF EXISTS
	`groups`;

-- Create new table
CREATE TABLE `groups` (
	`Id` bigint NOT NULL DEFAULT 0,
	`Member1` int(10) unsigned NULL DEFAULT NULL,
	`Member1` int(10) unsigned NULL DEFAULT NULL,
	`Member1` int(10) unsigned NULL DEFAULT NULL,
	`Member1` int(10) unsigned NULL DEFAULT NULL,
	`Member1` int(10) unsigned NULL DEFAULT NULL,
	PRIMARY KEY (`Id`)
);

-- Rest any rest of groups in the characters table

UPDATE
	`characters`
SET
	`GroupID` = NULL,
	`IsGroupMaster` = NULL;

