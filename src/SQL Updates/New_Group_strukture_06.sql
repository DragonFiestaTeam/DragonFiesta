CREATE TABLE `groups` (
  `Master` smallint(5) unsigned NOT NULL,
  `Member1` smallint(5) unsigned NOT NULL,
  `Member2` smallint(5) unsigned NOT NULL,
  `Member3` smallint(5) unsigned NOT NULL,
  `Member4` smallint(5) unsigned NOT NULL,
  `id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`Master`,`Member1`,`Member3`,`Member2`,`Member4`,`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

