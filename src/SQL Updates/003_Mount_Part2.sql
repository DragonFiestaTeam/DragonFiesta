ALTER TABLE `items`
ADD COLUMN `fuelcount`  int NOT NULL DEFAULT 0 AFTER `Amount`;

ALTER TABLE `characters`
ADD COLUMN `MountID` int(11) NOT NULL DEFAULT '65535' AFTER `IsGroupMaster`,
ADD COLUMN `MountFood` int(11) NOT NULL DEFAULT '0' AFTER `MountID`;
