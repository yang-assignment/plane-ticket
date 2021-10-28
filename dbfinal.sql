/*
 Navicat Premium Data Transfer

 Source Server         : test
 Source Server Type    : MySQL
 Source Server Version : 80021
 Source Host           : localhost:3306
 Source Schema         : dbfinal

 Target Server Type    : MySQL
 Target Server Version : 80021
 File Encoding         : 65001

 Date: 04/02/2021 19:16:13
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for admin
-- ----------------------------
DROP TABLE IF EXISTS `admin`;
CREATE TABLE `admin`  (
  `id` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `psw` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of admin
-- ----------------------------
INSERT INTO `admin` VALUES ('admin', '1');

-- ----------------------------
-- Table structure for customer
-- ----------------------------
DROP TABLE IF EXISTS `customer`;
CREATE TABLE `customer`  (
  `id` char(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `psw` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `name` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `phone` char(11) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `birth` date NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of customer
-- ----------------------------
INSERT INTO `customer` VALUES ('1', '2', '张三', '18052368700', '2020-12-29');
INSERT INTO `customer` VALUES ('aaa', 'bbb', '李四', '18072760626', '2020-12-29');

-- ----------------------------
-- Table structure for flight
-- ----------------------------
DROP TABLE IF EXISTS `flight`;
CREATE TABLE `flight`  (
  `flightid` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `planeid` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `lineid` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `begintime` datetime(0) NULL DEFAULT NULL,
  `endtime` datetime(0) NULL DEFAULT NULL,
  `price` decimal(10, 2) NULL DEFAULT NULL,
  `remaining` int NULL DEFAULT NULL,
  PRIMARY KEY (`flightid`) USING BTREE,
  INDEX `flight_ibfk_1`(`planeid`) USING BTREE,
  INDEX `flight_ibfk_2`(`lineid`) USING BTREE,
  CONSTRAINT `flight_ibfk_1` FOREIGN KEY (`planeid`) REFERENCES `plane` (`planeid`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `flight_ibfk_2` FOREIGN KEY (`lineid`) REFERENCES `line` (`lineid`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of flight
-- ----------------------------
INSERT INTO `flight` VALUES ('1002', '2', '1', '2020-12-29 21:23:18', '2020-12-30 02:23:22', 400.00, 20);
INSERT INTO `flight` VALUES ('1003', '2', '2', '2020-12-30 02:23:27', '2020-12-30 07:23:31', 400.00, 94);
INSERT INTO `flight` VALUES ('1004', '1', '2', '2020-12-16 09:43:58', '2020-12-03 09:44:00', 200.00, 8);

-- ----------------------------
-- Table structure for line
-- ----------------------------
DROP TABLE IF EXISTS `line`;
CREATE TABLE `line`  (
  `lineid` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `beginplace` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `endplace` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`lineid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of line
-- ----------------------------
INSERT INTO `line` VALUES ('1', '北京', '上海');
INSERT INTO `line` VALUES ('2', '上海', '昆明');

-- ----------------------------
-- Table structure for plane
-- ----------------------------
DROP TABLE IF EXISTS `plane`;
CREATE TABLE `plane`  (
  `planeid` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `company` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `seat` int NULL DEFAULT NULL,
  `type` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`planeid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of plane
-- ----------------------------
INSERT INTO `plane` VALUES ('1', '云南航空', 20, '波音737');
INSERT INTO `plane` VALUES ('2', '北京航空', 10, '空客');

-- ----------------------------
-- Table structure for ticket
-- ----------------------------
DROP TABLE IF EXISTS `ticket`;
CREATE TABLE `ticket`  (
  `ticketid` int NOT NULL AUTO_INCREMENT,
  `custid` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `flightid` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `type` char(3) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `price` decimal(10, 2) NULL DEFAULT NULL,
  `number` int NULL DEFAULT NULL,
  `detail` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`ticketid`) USING BTREE,
  INDEX `ticket_ibfk_1`(`flightid`) USING BTREE,
  INDEX `ticket_ibfk_2`(`custid`) USING BTREE,
  CONSTRAINT `ticket_ibfk_1` FOREIGN KEY (`flightid`) REFERENCES `flight` (`flightid`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ticket_ibfk_2` FOREIGN KEY (`custid`) REFERENCES `customer` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE = InnoDB AUTO_INCREMENT = 33 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of ticket
-- ----------------------------
INSERT INTO `ticket` VALUES (31, '1', '1004', '成人票', 200.00, 1, '上海-昆明');
INSERT INTO `ticket` VALUES (32, '1', '1003', '成人票', 400.00, 6, '上海-昆明');
INSERT INTO `ticket` VALUES (33, '1', '1004', '成人票', 200.00, 1, '上海-昆明');

SET FOREIGN_KEY_CHECKS = 1;
