ALTER TABLE `versions`
CHANGE COLUMN `version` `version_number`  int(11) NOT NULL DEFAULT 0 AFTER `year`;

RENAME TABLE hashs TO Hashes;

ALTER TABLE `hashes`
CHANGE COLUMN `hash_string` `hash_string`  varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '' AFTER `id`;

