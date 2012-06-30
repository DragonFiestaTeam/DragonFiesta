/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : fiesta_world

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-06-30 11:02:39
*/

SET FOREIGN_KEY_CHECKS=0;

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
INSERT INTO `guildmembers` VALUES ('44', '6', '9000', '99');
