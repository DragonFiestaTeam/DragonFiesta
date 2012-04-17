DELIMITER $$
CREATE PROCEDURE `update_equip` (
IN puniqueid BIGINT,
IN powner INT,
IN pslot SMALLINT,
IN pupgrades TINYINT,
IN pstr SMALLINT UNSIGNED,
IN pend SMALLINT UNSIGNED,
IN pdex SMALLINT UNSIGNED,
IN pspr SMALLINT UNSIGNED,
IN pint SMALLINT UNSIGNED)
BEGIN
UPDATE equips SET Slot=pslot,
Owner=powner,
Upgrades=pupgrades,
iSTR=pstr,
iEND=pend,
iDEX=pdex,
ispr=pspr,
iINT = pint
WHERE ID = puniqueid;
END $$

DROP PROCEDURE IF EXISTS `give_item` $$
CREATE PROCEDURE `give_item` (
OUT puniqueid BIGINT,
IN powner INT,
IN pslot TINYINT,
IN pitemid SMALLINT unsigned,
IN pamount SMALLINT)
BEGIN
INSERT INTO items (Owner, Slot, ItemID, Amount) VALUES (powner, pslot, pitemid, pamount);
SET puniqueid = LAST_INSERT_ID();
END $$
CREATE PROCEDURE `update_equip` (
IN puniqueid BIGINT,
IN powner INT,
IN pslot SMALLINT,
IN pupgrades TINYINT,
IN pstr SMALLINT UNSIGNED,
IN pend SMALLINT UNSIGNED,
IN pdex SMALLINT UNSIGNED,
IN pspr SMALLINT UNSIGNED,
IN pint SMALLINT UNSIGNED)
BEGIN
UPDATE equips SET Slot=pslot,
Owner=powner,
Upgrades=pupgrades,
iSTR=pstr,
iEND=pend,
iDEX=pdex,
ispr=pspr,
iINT = pint
WHERE ID = puniqueid;
END $$

DROP PROCEDURE IF EXISTS `update_item` $$
CREATE PROCEDURE `update_item` (
IN puniqueid BIGINT,
IN powner INT,
IN pslot TINYINT,
IN pamount SMALLINT)
BEGIN
UPDATE items SET Slot=pslot, Owner=powner, Amount=pamount
WHERE ID = puniqueid;
END $$

DROP PROCEDURE IF EXISTS `give_equip` $$
CREATE PROCEDURE `give_equip` (
OUT puniqueid BIGINT,
IN powner INT,
IN pslot SMALLINT,
IN pequipID SMALLINT unsigned)
BEGIN
INSERT INTO equips (Owner, Slot, EquipID) VALUES (powner, pslot, pequipid);
SET puniqueid = LAST_INSERT_ID();
END $$

DELIMITER ;