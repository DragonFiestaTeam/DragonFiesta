CREATE TABLE `items` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `Owner` int(11) unsigned NOT NULL DEFAULT '0',
  `Slot` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `ItemID` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Equipt` tinyint(4) NOT NULL DEFAULT '0',
  `Amount` smallint(5) unsigned NOT NULL,
  `fuelcount` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=156 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

