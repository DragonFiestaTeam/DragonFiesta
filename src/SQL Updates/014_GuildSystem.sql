/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : fiesta_world

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-07-08 13:53:53
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `academymembers`
-- ----------------------------
DROP TABLE IF EXISTS `academymembers`;
CREATE TABLE `academymembers` (
  `OwnerGuildID` int(11) DEFAULT NULL,
  `CharID` int(11) DEFAULT NULL,
  `Rank` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of academymembers
-- ----------------------------

-- ----------------------------
-- Table structure for `guild`
-- ----------------------------
DROP TABLE IF EXISTS `guild`;
CREATE TABLE `guild` (
  `ID` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(16) NOT NULL,
  `Password` text NOT NULL,
  `GuildMaster` varchar(10) NOT NULL,
  `GuildWar` tinyint(4) NOT NULL,
  `GuildAcademyMessage` varchar(0) NOT NULL DEFAULT '',
  `GuildMessage` varchar(0) NOT NULL DEFAULT '',
  `GuildMessageCreateDate` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  `GuildMessageCreater` varchar(0) NOT NULL DEFAULT '',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=100 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of guild
-- ----------------------------
INSERT INTO `guild` VALUES ('6', 'lol', 'cc', 'Create', '1', '', '', '0000-00-00 00:00:00', '');
INSERT INTO `guild` VALUES ('7', 'efvzjv', '???', 'Create', '1', '', '', '0000-00-00 00:00:00', '');

-- ----------------------------
-- Table structure for `guildmembers`
-- ----------------------------
DROP TABLE IF EXISTS `guildmembers`;
CREATE TABLE `guildmembers` (
  `CharID` int(11) NOT NULL,
  `Rank` tinyint(4) NOT NULL,
  `Korp` smallint(6) NOT NULL,
  `GuildID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of guildmembers
-- ----------------------------
INSERT INTO `guildmembers` VALUES ('50', '0', '0', '5');
INSERT INTO `guildmembers` VALUES ('50', '0', '0', '6');
INSERT INTO `guildmembers` VALUES ('50', '0', '0', '7');
