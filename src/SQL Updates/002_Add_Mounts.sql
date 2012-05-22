DROP TABLE IF EXISTS `fiesta_data`.`mounts`;

CREATE TABLE `fiesta_data`.`mounts` (
  `Handle` tinyint(3) NOT NULL,
  `ItemID` int(11) NOT NULL,
  `TickSpeed` bigint(20) NOT NULL,
  `Level` tinyint(3) NOT NULL,
  `Food` int(11) NOT NULL DEFAULT '100',
  `Speed` int(4) NOT NULL DEFAULT '165',
  PRIMARY KEY (`Handle`,`ItemID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

ALTER TABLE `fiesta_world`.`characters`
ADD COLUMN `MountID`  int NOT NULL DEFAULT 0 AFTER `IsGroupMaster`;

