/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : fiesta_data

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-06-28 10:15:05
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `masterrewards`
-- ----------------------------
DROP TABLE IF EXISTS `masterrewards`;
CREATE TABLE `masterrewards` (
  `ItemID` smallint(6) NOT NULL,
  `Level` tinyint(4) NOT NULL,
  `Job` tinyint(4) NOT NULL,
  `Count` tinyint(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of masterrewards
-- ----------------------------
INSERT INTO `masterrewards` VALUES ('250', '3', '1', '0');

-- ----------------------------
-- Table structure for `masterrewardstates`
-- ----------------------------
DROP TABLE IF EXISTS `masterrewardstates`;
CREATE TABLE `masterrewardstates` (
  `ItemID` smallint(6) DEFAULT NULL,
  `Upgrades` tinyint(4) NOT NULL,
  `Str` smallint(6) NOT NULL,
  `End` smallint(6) NOT NULL,
  `Dex` smallint(6) NOT NULL,
  `Int` smallint(6) NOT NULL,
  `Spr` smallint(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of masterrewardstates
-- ----------------------------
INSERT INTO `masterrewardstates` VALUES ('250', '0', '0', '0', '9', '0', '0');
