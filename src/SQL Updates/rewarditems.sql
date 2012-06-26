/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : fiesta_world

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-06-26 17:03:36
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `rewarditems`
-- ----------------------------
DROP TABLE IF EXISTS `rewarditems`;
CREATE TABLE `rewarditems` (
  `CharID` int(11) NOT NULL,
  `Slot` tinyint(4) NOT NULL,
  `ItemID` int(11) NOT NULL,
  `PageID` tinyint(4) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of rewarditems
-- ----------------------------
INSERT INTO `rewarditems` VALUES ('39', '1', '46006', '0');
INSERT INTO `rewarditems` VALUES ('39', '0', '46008', '0');
