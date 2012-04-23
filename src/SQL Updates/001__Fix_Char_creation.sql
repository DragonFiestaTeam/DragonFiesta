ALTER TABLE 
	`fiesta_world`.`characters` 
CHANGE 
	COLUMN `GroupID` 
		`GroupID` INT(11) UNSIGNED NULL DEFAULT NULL ;

ALTER TABLE 
	`fiesta_world`.`characters` 
CHANGE 
	COLUMN 
		`IsGroupMaster` `IsGroupMaster` TINYINT(1) UNSIGNED NULL DEFAULT NULL  ;
		
UPDATE
	`fiesta_world`.`characters`
SET
	`GroupID` = NULL,
	`IsGroupMaster` = NULL
WHERE
	`GroupID` = 0
AND `IsGroupMaster` = 0;