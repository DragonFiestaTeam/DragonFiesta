/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : char

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-02-05 18:40:40
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `groups`
-- ----------------------------
DROP TABLE IF EXISTS `groups`;
CREATE TABLE `groups` (
  `Master` varbinary(20) NOT NULL DEFAULT '',
  `Member1` varchar(16) NOT NULL DEFAULT '',
  `Member2` varchar(16) NOT NULL DEFAULT '',
  `Member3` varchar(16) NOT NULL DEFAULT '',
  `Member4` varchar(16) NOT NULL DEFAULT '',
  PRIMARY KEY (`Master`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
