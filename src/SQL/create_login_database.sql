USE `test`;
/******************************************************************************/
DROP SCHEMA IF EXISTS `df_login`;
CREATE SCHEMA `df_login`;
/******************************************************************************/

DROP TABLE IF EXISTS accounts;
CREATE TABLE accounts (
	id
		BIGINT
		NOT NULL AUTO_INCREMENT,
	`name`
		VARCHAR(20)
		NOT NULL DEFAULT '',
	`password`
		VARCHAR(20)
		NOT NULL DEFAULT '',
	access_level
		BIGINT
		NOT NULL DEFAULT 0,
	PRIMARY KEY(id)
);
DROP TABLE IF EXISTS access_levels;
CREATE TABLE access_levels (
	id
		BIGINT
		NOT NULL AUTO_INCREMENT,
	name
		VARCHAR(100)
		NOT NULL DEFAULT '',
	can_login
		BIT
		NOT NULL DEFAULT 0,
	PRIMARY KEY(id)
);
DROP TABLE IF EXISTS hashs;
CREATE TABLE hashs (
	id
		BIGINT
		NOT NULL AUTO_INCREMENT,
	`hash`
		varchar(20)
		NOT NULL DEFAULT '',
	allow_login
		BIT
		NOT NULL DEFAULT 0,
	PRIMARY KEY(id)
);
DROP TABLE IF EXISTS versions;
CREATE TABLE versions (
	id
		BIGINT
		NOT NULL AUTO_INCREMENT,
	`year`
		INT 
		NOT NULL DEFAULT 0,
	`version`
		INT
		NOT NULL DEFAULT 0,
	`allowed`
		BIT
		NOT NULL DEFAULT 0,
	PRIMARY KEY(id),
);
/******************************************************************************/
ALTER TABLE accounts
	ADD CONSTRAINT
		FOREIGN KEY FK_access_level 
			( access_level )
			REFERENCES access_levels.id;
/******************************************************************************/
CREATE VIEW v_Auth AS 
	SELECT 
		accounts.id,
		accounts.`name`,
		accounts.`password`,
		access_levels.can_login
	FROM
		accounts
		INNER JOIN access_levels
		ON accounts.access_level = access_levels.id;

		