/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : fiesta_data

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-06-26 21:09:56
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
  `Upgrades` tinyint(4) NOT NULL,
  `Str` smallint(6) NOT NULL,
  `End` smallint(6) NOT NULL,
  `Dex` smallint(6) NOT NULL,
  `Int` smallint(6) NOT NULL,
  `Spr` smallint(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of masterrewards
-- ----------------------------
INSERT INTO `masterrewards` VALUES ('0', '3', '1', '1', '1', '1', '1', '1', '1');
