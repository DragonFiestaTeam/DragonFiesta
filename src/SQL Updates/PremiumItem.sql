/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : fiesta_world

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-06-26 13:40:53
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `premiumitem`
-- ----------------------------
DROP TABLE IF EXISTS `premiumitem`;
CREATE TABLE `premiumitem` (
  `CharID` int(11) NOT NULL,
  `ShopID` int(11) NOT NULL,
  `UniqueID` int(11) NOT NULL,
  `PageID` tinyint(4) NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of premiumitem
-- ----------------------------
