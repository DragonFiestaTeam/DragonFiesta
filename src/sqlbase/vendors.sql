/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50516
Source Host           : localhost:3306
Source Database       : data

Target Server Type    : MYSQL
Target Server Version : 50516
File Encoding         : 65001

Date: 2011-11-27 21:04:28
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `vendors`
-- ----------------------------
DROP TABLE IF EXISTS `vendors`;
CREATE TABLE `vendors` (
  `NPCID` text NOT NULL,
  `ItemID` bigint(11) NOT NULL,
  `InvSlot` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of vendors
-- ----------------------------
INSERT INTO `vendors` VALUES ('RouFurnitureForestTom', '50001', '1');
INSERT INTO `vendors` VALUES ('RouFurnitureForestTom', '50003', '2');
INSERT INTO `vendors` VALUES ('RouFurnitureForestTom', '50007', '3');
INSERT INTO `vendors` VALUES ('RouFurnitureForestTom', '50004', '4');
INSERT INTO `vendors` VALUES ('RouFurnitureForestTom', '50005', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '4700', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '4701', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '4720', '7');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '4721', '12');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '4780', '18');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '4740', '19');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '4741', '24');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '4760', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7600', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7609', '7');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7610', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '5440', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '5441', '2');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '5442', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '5400', '7');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '5401', '12');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '5460', '18');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '5660', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7700', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7701', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7718', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6200', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6201', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6220', '7');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6221', '12');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6260', '18');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6320', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7800', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7801', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7818', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6901', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6902', '2');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6903', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6920', '7');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6921', '8');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6922', '12');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6940', '13');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6941', '14');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6942', '18');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6960', '19');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6961', '20');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '6962', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7900', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7901', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '7918', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19400', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19401', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19420', '7');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19421', '12');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19440', '13');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19441', '18');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19460', '19');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19461', '24');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19480', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19830', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19831', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '19870', '0');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '29001', '1');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '29206', '6');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20000', '7');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20001', '8');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23021', '9');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23022', '10');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24023', '11');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24050', '12');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20005', '13');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20006', '14');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23049', '15');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23073', '16');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24074', '17');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24080', '18');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20010', '19');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20011', '20');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23061', '21');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23079', '22');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24003', '23');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24006', '24');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20100', '25');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20101', '26');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23026', '27');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23055', '28');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24012', '29');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24071', '30');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20105', '31');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20106', '32');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23003', '33');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23006', '34');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24042', '35');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24068', '36');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20110', '37');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20111', '38');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23043', '39');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23070', '40');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24026', '41');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24045', '42');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20115', '43');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20116', '44');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23064', '45');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23062', '46');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24065', '47');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '24019', '48');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20500', '49');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20501', '50');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '23017', '54');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20505', '55');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20506', '60');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20510', '61');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20511', '66');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20515', '67');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20516', '72');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20525', '73');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20526', '78');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20530', '79');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20531', '84');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20554', '85');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20551', '90');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20800', '96');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20803', '102');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '20806', '108');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '21000', '114');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '21003', '120');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '21006', '121');
INSERT INTO `vendors` VALUES ('RouSkillRubi', '21007', '0');
INSERT INTO `vendors` VALUES ('RouWeaponTitleMctZach', '10000', '1');
INSERT INTO `vendors` VALUES ('RouWeaponTitleMctZach', '10001', '2');
INSERT INTO `vendors` VALUES ('RouWeaponTitleMctZach', '10002', '6');
INSERT INTO `vendors` VALUES ('RouWeaponTitleMctZach', '10003', '7');
INSERT INTO `vendors` VALUES ('RouWeaponTitleMctZach', '10004', '8');
INSERT INTO `vendors` VALUES ('RouWeaponTitleMctZach', '10005', '0');
INSERT INTO `vendors` VALUES ('RouSmithJames', '250', '1');
INSERT INTO `vendors` VALUES ('RouSmithJames', '251', '2');
INSERT INTO `vendors` VALUES ('RouSmithJames', '252', '6');
INSERT INTO `vendors` VALUES ('RouSmithJames', '750', '7');
INSERT INTO `vendors` VALUES ('RouSmithJames', '751', '8');
INSERT INTO `vendors` VALUES ('RouSmithJames', '752', '12');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1250', '13');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1251', '14');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1252', '18');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1750', '19');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1751', '20');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1752', '24');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57363', '25');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57364', '26');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57365', '0');
INSERT INTO `vendors` VALUES ('RouSmithJames', '0', '1');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1', '2');
INSERT INTO `vendors` VALUES ('RouSmithJames', '2', '3');
INSERT INTO `vendors` VALUES ('RouSmithJames', '3', '6');
INSERT INTO `vendors` VALUES ('RouSmithJames', '4', '7');
INSERT INTO `vendors` VALUES ('RouSmithJames', '5', '8');
INSERT INTO `vendors` VALUES ('RouSmithJames', '6', '12');
INSERT INTO `vendors` VALUES ('RouSmithJames', '200', '13');
INSERT INTO `vendors` VALUES ('RouSmithJames', '201', '18');
INSERT INTO `vendors` VALUES ('RouSmithJames', '500', '19');
INSERT INTO `vendors` VALUES ('RouSmithJames', '501', '20');
INSERT INTO `vendors` VALUES ('RouSmithJames', '502', '21');
INSERT INTO `vendors` VALUES ('RouSmithJames', '503', '24');
INSERT INTO `vendors` VALUES ('RouSmithJames', '504', '25');
INSERT INTO `vendors` VALUES ('RouSmithJames', '505', '26');
INSERT INTO `vendors` VALUES ('RouSmithJames', '506', '30');
INSERT INTO `vendors` VALUES ('RouSmithJames', '700', '31');
INSERT INTO `vendors` VALUES ('RouSmithJames', '701', '36');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1000', '37');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1001', '38');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1002', '39');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1003', '42');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1004', '43');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1005', '44');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1006', '48');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1500', '49');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1501', '50');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1502', '51');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1503', '54');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1504', '55');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1505', '56');
INSERT INTO `vendors` VALUES ('RouSmithJames', '1506', '60');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57390', '61');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57391', '62');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57392', '66');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57393', '67');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57394', '68');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57395', '69');
INSERT INTO `vendors` VALUES ('RouSmithJames', '57396', '0');
INSERT INTO `vendors` VALUES ('RouSmithJames', '2000', '1');
INSERT INTO `vendors` VALUES ('RouSmithJames', '2001', '2');
INSERT INTO `vendors` VALUES ('RouSmithJames', '2002', '3');
INSERT INTO `vendors` VALUES ('RouSmithJames', '2003', '4');
INSERT INTO `vendors` VALUES ('RouSmithJames', '2004', '6');
INSERT INTO `vendors` VALUES ('RouSmithJames', '2300', '7');
INSERT INTO `vendors` VALUES ('RouSmithJames', '2301', '8');
INSERT INTO `vendors` VALUES ('RouSmithJames', '2302', '0');
INSERT INTO `vendors` VALUES ('RouItemMctPey', '8000', '3');
INSERT INTO `vendors` VALUES ('RouItemMctPey', '4500', '4');
INSERT INTO `vendors` VALUES ('RouItemMctPey', '3097', '5');
INSERT INTO `vendors` VALUES ('RouItemMctPey', '3114', '6');
INSERT INTO `vendors` VALUES ('RouItemMctPey', '2522', '12');
INSERT INTO `vendors` VALUES ('RouItemMctPey', '38043', '18');
INSERT INTO `vendors` VALUES ('RouItemMctPey', '31037', '0');
