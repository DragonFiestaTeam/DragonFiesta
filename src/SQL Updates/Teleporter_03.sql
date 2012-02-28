ALTER TABLE teleporter ADD COLUMN `AnswerMap0X` INT( 11 ) NOT NULL ;
ALTER TABLE teleporter ADD COLUMN `AnswerMap0Y` INT( 11 ) NOT NULL ;
INSERT INTO `teleporter` (`NPCId`, `AnswerMap0`, `AnswerMap1`, `AnswerMap2`, `AnswerMap3`, `AnswerMap0X`, `AnswerMap0Y`, `AnswerMap1X`, `AnswerMap1Y`, `AnswerMap2X`, `AnswerMap2Y`, `AnswerMap3X`, `AnswerMap3Y`) VALUES
(191, '0', '9', '75', '5', 4150, 4744, 11788, 10395, 9039, 9312, 13660, 7862);
UPDATE shinenpc SET Flags='2' WHERE MobID='191';