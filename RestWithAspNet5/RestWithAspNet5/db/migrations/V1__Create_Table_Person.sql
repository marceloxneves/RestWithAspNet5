CREATE TABLE `person` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `address` varchar(100) DEFAULT NULL,
  `first_name` varchar(100) DEFAULT NULL,
  `last_name` varchar(100) DEFAULT NULL,
  `gender` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id`)
)
