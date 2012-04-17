/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : fiesta_world

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-04-17 20:10:28
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `equips`
-- ----------------------------
DROP TABLE IF EXISTS `equips`;
CREATE TABLE `equips` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `Owner` int(10) unsigned NOT NULL DEFAULT '0',
  `Slot` smallint(6) NOT NULL DEFAULT '0',
  `EquipID` smallint(5) unsigned NOT NULL,
  `Upgrades` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `iSTR` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `iEND` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `iDEX` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `iSPR` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `iINT` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `Equiptet` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=72 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of equips
-- ----------------------------

-- ----------------------------
-- Table structure for `items`
-- ----------------------------
DROP TABLE IF EXISTS `items`;
CREATE TABLE `items` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `Owner` int(11) unsigned NOT NULL DEFAULT '0',
  `Slot` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `ItemID` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Amount` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of items
-- ----------------------------
