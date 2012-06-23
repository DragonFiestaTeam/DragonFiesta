/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : fiesta_world

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-06-23 20:32:42
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `masters`
-- ----------------------------
DROP TABLE IF EXISTS `masters`;
CREATE TABLE `masters` (
  `CharID` bigint(20) unsigned NOT NULL,
  `MemberName` varchar(16) NOT NULL,
  `Level` tinyint(3) unsigned NOT NULL,
  `RegisterDate` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of masters
-- ----------------------------
