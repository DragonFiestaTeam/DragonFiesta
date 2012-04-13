CREATE TABLE `friends` (
  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `CharID` int(10) unsigned NOT NULL,
  `FriendID` int(10) unsigned NOT NULL,
  `Pending` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `LastConnectDay` tinyint(3) NOT NULL DEFAULT '0',
  `LastConnectMonth` tinyint(3) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

