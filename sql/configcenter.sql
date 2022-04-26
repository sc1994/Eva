﻿--
-- Script was generated by Devart dbForge Studio 2020 for MySQL, Version 9.0.791.0
-- Product home page: http://www.devart.com/dbforge/mysql/studio
-- Script date 2022/4/26 10:02:59
-- Server version: 8.0.27
-- Client version: 4.1
--

-- 
-- Disable foreign keys
-- 
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;

-- 
-- Set SQL mode
-- 
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 
-- Set character set the client will use to send SQL statements to the server
--
SET NAMES 'utf8';

--
-- Set default database
--
USE configcenter;

--
-- Drop table `agc_app`
--
DROP TABLE IF EXISTS agc_app;

--
-- Drop table `agc_appinheritanced`
--
DROP TABLE IF EXISTS agc_appinheritanced;

--
-- Drop table `agc_config`
--
DROP TABLE IF EXISTS agc_config;

--
-- Drop table `agc_config_published`
--
DROP TABLE IF EXISTS agc_config_published;

--
-- Drop table `agc_publish_detail`
--
DROP TABLE IF EXISTS agc_publish_detail;

--
-- Drop table `agc_publish_timeline`
--
DROP TABLE IF EXISTS agc_publish_timeline;

--
-- Drop table `agc_server_node`
--
DROP TABLE IF EXISTS agc_server_node;

--
-- Drop table `agc_setting`
--
DROP TABLE IF EXISTS agc_setting;

--
-- Drop table `agc_sys_log`
--
DROP TABLE IF EXISTS agc_sys_log;

--
-- Drop table `agc_user`
--
DROP TABLE IF EXISTS agc_user;

--
-- Drop table `agc_user_app_auth`
--
DROP TABLE IF EXISTS agc_user_app_auth;

--
-- Drop table `agc_user_role`
--
DROP TABLE IF EXISTS agc_user_role;

--
-- Set default database
--
USE configcenter;

--
-- Create table `agc_user_role`
--
CREATE TABLE agc_user_role (
  id VARCHAR(36) NOT NULL,
  user_id VARCHAR(50) DEFAULT NULL,
  role ENUM('SuperAdmin','Admin','NormalUser') NOT NULL,
  create_time DATETIME(3) NOT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_user_app_auth`
--
CREATE TABLE agc_user_app_auth (
  id VARCHAR(36) NOT NULL,
  app_id VARCHAR(36) DEFAULT NULL,
  user_id VARCHAR(36) DEFAULT NULL,
  permission VARCHAR(50) DEFAULT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_user`
--
CREATE TABLE agc_user (
  id VARCHAR(36) NOT NULL,
  user_name VARCHAR(50) DEFAULT NULL,
  password VARCHAR(50) DEFAULT NULL,
  salt VARCHAR(36) DEFAULT NULL,
  team VARCHAR(50) DEFAULT NULL,
  create_time DATETIME(3) NOT NULL,
  update_time DATETIME(3) DEFAULT NULL,
  status ENUM('Normal','Deleted') NOT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_sys_log`
--
CREATE TABLE agc_sys_log (
  id INT NOT NULL AUTO_INCREMENT,
  app_id VARCHAR(36) DEFAULT NULL,
  log_type ENUM('Normal','Warn') NOT NULL,
  log_time DATETIME(3) DEFAULT NULL,
  log_text VARCHAR(2000) DEFAULT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
AUTO_INCREMENT = 9,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_setting`
--
CREATE TABLE agc_setting (
  id VARCHAR(36) NOT NULL,
  value VARCHAR(200) DEFAULT NULL,
  create_time DATETIME(3) NOT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 16384,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_server_node`
--
CREATE TABLE agc_server_node (
  address VARCHAR(100) NOT NULL,
  remark VARCHAR(50) DEFAULT NULL,
  status ENUM('Offline','Online') NOT NULL,
  last_echo_time DATETIME(3) DEFAULT NULL,
  create_time DATETIME(3) NOT NULL,
  PRIMARY KEY (address)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_publish_timeline`
--
CREATE TABLE agc_publish_timeline (
  id VARCHAR(36) NOT NULL,
  app_id VARCHAR(36) DEFAULT NULL,
  publish_time DATETIME(3) DEFAULT NULL,
  publish_user_id VARCHAR(36) DEFAULT NULL,
  publish_user_name VARCHAR(50) DEFAULT NULL,
  version INT NOT NULL,
  log VARCHAR(100) DEFAULT NULL,
  env VARCHAR(50) DEFAULT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_publish_detail`
--
CREATE TABLE agc_publish_detail (
  id VARCHAR(36) NOT NULL,
  app_id VARCHAR(36) DEFAULT NULL,
  Version INT NOT NULL,
  publish_timeline_id VARCHAR(36) DEFAULT NULL,
  config_id VARCHAR(36) DEFAULT NULL,
  g VARCHAR(100) DEFAULT NULL,
  k VARCHAR(100) DEFAULT NULL,
  v VARCHAR(4000) DEFAULT NULL,
  description VARCHAR(200) DEFAULT NULL,
  edit_status ENUM('Add','Edit','Deleted','Commit') NOT NULL,
  env VARCHAR(50) DEFAULT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_config_published`
--
CREATE TABLE agc_config_published (
  id VARCHAR(36) NOT NULL,
  app_id VARCHAR(36) DEFAULT NULL,
  g VARCHAR(100) DEFAULT NULL,
  k VARCHAR(100) DEFAULT NULL,
  v VARCHAR(4000) DEFAULT NULL,
  publish_time DATETIME(3) DEFAULT NULL,
  config_id VARCHAR(36) DEFAULT NULL,
  publish_timeline_id VARCHAR(36) DEFAULT NULL,
  version INT NOT NULL,
  status ENUM('Deleted','Enabled') NOT NULL,
  env VARCHAR(50) DEFAULT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_config`
--
CREATE TABLE agc_config (
  id VARCHAR(36) NOT NULL,
  app_id VARCHAR(36) DEFAULT NULL,
  g VARCHAR(100) DEFAULT NULL,
  k VARCHAR(100) DEFAULT NULL,
  v VARCHAR(4000) DEFAULT NULL,
  description VARCHAR(200) DEFAULT NULL,
  create_time DATETIME(3) NOT NULL,
  update_time DATETIME(3) DEFAULT NULL,
  status ENUM('Deleted','Enabled') NOT NULL,
  online_status ENUM('WaitPublish','Online') NOT NULL,
  edit_status ENUM('Add','Edit','Deleted','Commit') NOT NULL,
  env VARCHAR(50) DEFAULT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_appinheritanced`
--
CREATE TABLE agc_appinheritanced (
  id VARCHAR(36) NOT NULL,
  appid VARCHAR(36) DEFAULT NULL,
  inheritanced_appid VARCHAR(36) DEFAULT NULL,
  sort INT NOT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

--
-- Create table `agc_app`
--
CREATE TABLE agc_app (
  id VARCHAR(36) NOT NULL,
  name VARCHAR(50) DEFAULT NULL,
  `group` VARCHAR(50) DEFAULT NULL,
  secret VARCHAR(36) DEFAULT NULL,
  create_time DATETIME(3) NOT NULL,
  update_time DATETIME(3) DEFAULT NULL,
  enabled BIT(1) NOT NULL,
  type ENUM('PRIVATE','Inheritance') NOT NULL,
  app_admin VARCHAR(36) DEFAULT NULL,
  PRIMARY KEY (id)
)
ENGINE = INNODB,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci;

-- 
-- Dumping data for table agc_user_role
--
INSERT INTO agc_user_role VALUES
('6136e6e9bb5c4636b739e1552ddb7018', 'super_admin', 'Admin', '0001-01-01 00:00:00'),
('bd4b876e061b452c8439cbc4581f1863', 'super_admin', 'SuperAdmin', '0001-01-01 00:00:00');

-- 
-- Dumping data for table agc_user_app_auth
--
-- Table configcenter.agc_user_app_auth does not contain any data (it is empty)

-- 
-- Dumping data for table agc_user
--
INSERT INTO agc_user VALUES
('super_admin', 'admin', '7AA45C15EE5FDDE45B032C2A4C3C3EFE', '0cef4041edf34c3aa9230acac0ab142a', '', '2022-04-25 20:58:01.298', NULL, 'Normal');

-- 
-- Dumping data for table agc_sys_log
--
INSERT INTO agc_sys_log VALUES
(1, NULL, 'Normal', '2022-04-25 20:58:01.314', '超级管理员密码初始化成功'),
(2, NULL, 'Normal', '2022-04-25 20:58:08.148', 'admin 登录成功'),
(3, NULL, 'Normal', '2022-04-25 20:58:12.009', '用户：admin 添加节点：http://localhost:5005'),
(4, NULL, 'Normal', '2022-04-25 20:58:46.13', '用户：admin 新增应用【AppId：PublicID】【AppName：Public】'),
(5, NULL, 'Normal', '2022-04-25 20:59:21.57', '用户：admin 新增应用【AppId：DemandsID】【AppName：Demands】'),
(6, NULL, 'Normal', '2022-04-25 21:00:17.696', '用户：admin 新增应用【AppId：HttpAggregatorID】【AppName：HttpAggregator】'),
(7, 'PublicID', 'Normal', '2022-04-25 21:00:39.77', '用户：admin 新增配置【Group：】【Key：SeqServerUrl】【AppId：PublicID】【Env：DEV】【待发布】'),
(8, 'PublicID', 'Normal', '2022-04-25 21:00:43.008', '用户：admin 发布配置【AppId：PublicID】【Env：DEV】【版本：20220425210042】');

-- 
-- Dumping data for table agc_setting
--
INSERT INTO agc_setting VALUES
('environment', 'DEV,TEST,STAGING,PROD', '2022-04-25 21:11:31.348');

-- 
-- Dumping data for table agc_server_node
--
INSERT INTO agc_server_node VALUES
('http://localhost:5005', '控制台节点', 'Offline', NULL, '2022-04-25 20:58:11.995');

-- 
-- Dumping data for table agc_publish_timeline
--
INSERT INTO agc_publish_timeline VALUES
('1a0bc963d23f41c4b3fc4fe48f23eb8e', 'PublicID', '2022-04-25 21:00:42.952', 'super_admin', 'admin', 1, '', 'DEV');

-- 
-- Dumping data for table agc_publish_detail
--
INSERT INTO agc_publish_detail VALUES
('706cb51755b645dfa28db98db78170e3', 'PublicID', 1, '1a0bc963d23f41c4b3fc4fe48f23eb8e', 'ecde6610b3a241cea566090920b3e860', NULL, 'SeqServerUrl', 'http://seq', NULL, 'Add', 'DEV');

-- 
-- Dumping data for table agc_config_published
--
INSERT INTO agc_config_published VALUES
('6f7cbc8fe200482a97b5494062c33ad8', 'PublicID', NULL, 'SeqServerUrl', 'http://seq', '2022-04-25 21:00:42.952', 'ecde6610b3a241cea566090920b3e860', '1a0bc963d23f41c4b3fc4fe48f23eb8e', 1, 'Enabled', 'DEV');

-- 
-- Dumping data for table agc_config
--
INSERT INTO agc_config VALUES
('ecde6610b3a241cea566090920b3e860', 'PublicID', NULL, 'SeqServerUrl', 'http://seq', NULL, '2022-04-25 21:00:39.757', NULL, 'Enabled', 'Online', 'Commit', 'DEV');

-- 
-- Dumping data for table agc_appinheritanced
--
INSERT INTO agc_appinheritanced VALUES
('4cad2c0d97dd44e4bfb7f1485f134feb', 'HttpAggregatorID', 'PublicID', 0),
('72ec6ad7c1af422a8e08aabd09eccccc', 'DemandsID', 'PublicID', 0);

-- 
-- Dumping data for table agc_app
--
INSERT INTO agc_app VALUES
('DemandsID', 'Demands', NULL, 'Password@1', '2022-04-25 20:59:21.56', NULL, True, 'PRIVATE', 'super_admin'),
('HttpAggregatorID', 'HttpAggregator', NULL, 'Password@1', '2022-04-25 21:00:17.689', NULL, True, 'PRIVATE', 'super_admin'),
('PublicID', 'Public', NULL, NULL, '2022-04-25 20:58:46.113', NULL, True, 'Inheritance', 'super_admin');

-- 
-- Restore previous SQL mode
-- 
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;

-- 
-- Enable foreign keys
-- 
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;