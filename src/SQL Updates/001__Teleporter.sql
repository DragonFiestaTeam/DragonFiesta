/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : fiesta_data

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2012-02-28 13:15:26
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `teleporter`
-- ----------------------------
DROP TABLE IF EXISTS `teleporter`;
CREATE TABLE `teleporter` (
  `NPCId` int(11) NOT NULL,
  `AnswerMap1` varchar(255) NOT NULL,
  `AnswerMap2` varchar(255) NOT NULL,
  `AnswerMap3` varchar(255) NOT NULL,
  `AnswerMap1X` int(11) NOT NULL,
  `AnswerMap1Y` int(11) NOT NULL,
  `AnswerMap2X` int(11) NOT NULL,
  `AnswerMap2Y` int(11) NOT NULL,
  `AnswerMap3X` int(11) NOT NULL,
  `AnswerMap3y` int(11) NOT NULL,
  PRIMARY KEY (`NPCId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of teleporter
-- ----------------------------
