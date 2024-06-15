-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: nutriplan
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `admins`
--

DROP TABLE IF EXISTS `admins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `admins` (
  `Admins_ID` int NOT NULL AUTO_INCREMENT,
  `Firstname` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Surname` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Patronymic` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Login` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Password` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  PRIMARY KEY (`Admins_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admins`
--

LOCK TABLES `admins` WRITE;
/*!40000 ALTER TABLE `admins` DISABLE KEYS */;
INSERT INTO `admins` VALUES (4,'Анна','Куратова','Евгеньевна','kuratova_ae','supportkurat24'),(5,'Мария','Ярулина','Закиевна','yarulina_mz','supportyarul24'),(6,'Тест','Тестович','Тест','test','123');
/*!40000 ALTER TABLE `admins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `answer_variant`
--

DROP TABLE IF EXISTS `answer_variant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `answer_variant` (
  `User_answer_ID` bigint NOT NULL AUTO_INCREMENT,
  `1` tinyint NOT NULL DEFAULT '1',
  `2` tinyint NOT NULL DEFAULT '1',
  `3` tinyint NOT NULL DEFAULT '1',
  `4` tinyint NOT NULL DEFAULT '1',
  `5` tinyint NOT NULL DEFAULT '1',
  `6` tinyint NOT NULL DEFAULT '1',
  `7` tinyint NOT NULL DEFAULT '1',
  `8` tinyint NOT NULL DEFAULT '1',
  `9` tinyint NOT NULL DEFAULT '1',
  `10` tinyint NOT NULL DEFAULT '1',
  `11` tinyint NOT NULL DEFAULT '1',
  `12` tinyint NOT NULL DEFAULT '1',
  PRIMARY KEY (`User_answer_ID`),
  UNIQUE KEY `User_anwer_ID_UNIQUE` (`User_answer_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5410944163 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `answer_variant`
--

LOCK TABLES `answer_variant` WRITE;
/*!40000 ALTER TABLE `answer_variant` DISABLE KEYS */;
INSERT INTO `answer_variant` VALUES (400854468,0,0,0,0,0,1,1,0,0,0,0,0),(571254417,1,0,0,1,0,0,1,0,1,0,1,1),(575632196,0,0,0,0,0,0,0,0,0,0,0,0),(984814639,0,0,0,0,1,1,0,0,0,0,0,0),(1038282804,1,0,1,0,0,0,1,0,1,1,1,1),(1590207474,1,0,0,0,1,1,1,1,0,1,1,1),(5410944162,1,0,1,0,0,1,0,1,0,1,0,1);
/*!40000 ALTER TABLE `answer_variant` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dishes`
--

DROP TABLE IF EXISTS `dishes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dishes` (
  `Dishes_ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Energy_value` float NOT NULL,
  `Type_of_dish` int NOT NULL,
  `Meal_type` bigint NOT NULL,
  PRIMARY KEY (`Dishes_ID`),
  KEY `Type_of_dish_idx` (`Type_of_dish`),
  KEY `Type_of_meal_idx` (`Meal_type`),
  CONSTRAINT `Type_of_dish` FOREIGN KEY (`Type_of_dish`) REFERENCES `types_of_dishes` (`Type_ID`),
  CONSTRAINT `Type_of_meal` FOREIGN KEY (`Meal_type`) REFERENCES `meal_types` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dishes`
--

LOCK TABLES `dishes` WRITE;
/*!40000 ALTER TABLE `dishes` DISABLE KEYS */;
INSERT INTO `dishes` VALUES (1,'Суп-пюре из брокколи',43,1,1),(2,'Гречка с куриным филе',90.6,2,2),(3,'Творожная запеканка',168,3,3),(4,'Чизкейк',321,9,2),(7,'Молочный суп',111,3,1),(9,'Чай с молоком',35,9,1),(13,'Манная каша',95,3,1),(14,'Жульен',204,2,3),(15,'Тушеная говядина',224,2,3),(18,'Запеченное мясо',266,2,2);
/*!40000 ALTER TABLE `dishes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ingredients`
--

DROP TABLE IF EXISTS `ingredients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingredients` (
  `Ingredients_ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Energy_value` float NOT NULL,
  PRIMARY KEY (`Ingredients_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ingredients`
--

LOCK TABLES `ingredients` WRITE;
/*!40000 ALTER TABLE `ingredients` DISABLE KEYS */;
INSERT INTO `ingredients` VALUES (1,'Брокколи',34),(2,'Гречка',118),(3,'Куриное филе',165),(4,'Творог',120),(5,'Молоко',42),(6,'Вермишель',331),(7,'Сахар',387),(9,'Хлеб',40),(10,'Манная крупа',53),(11,'Грибы',39),(12,'Картофель',101),(13,'Говядина',123);
/*!40000 ALTER TABLE `ingredients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ingredients_dishes`
--

DROP TABLE IF EXISTS `ingredients_dishes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingredients_dishes` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Ingredients_id` int NOT NULL,
  `Dishes_id` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_Ingredients_id_idx` (`Ingredients_id`),
  KEY `fk_Dishes_id_idx` (`Dishes_id`),
  CONSTRAINT `fk_Dishes_id` FOREIGN KEY (`Dishes_id`) REFERENCES `dishes` (`Dishes_ID`),
  CONSTRAINT `fk_Ingredients_id` FOREIGN KEY (`Ingredients_id`) REFERENCES `ingredients` (`Ingredients_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ingredients_dishes`
--

LOCK TABLES `ingredients_dishes` WRITE;
/*!40000 ALTER TABLE `ingredients_dishes` DISABLE KEYS */;
INSERT INTO `ingredients_dishes` VALUES (1,1,1),(2,2,2),(3,3,2),(4,4,3),(7,5,7),(8,6,7),(9,7,7),(13,5,9),(14,10,13),(15,5,13),(16,3,14),(17,11,14),(18,13,15),(19,12,15),(23,3,18),(24,12,18);
/*!40000 ALTER TABLE `ingredients_dishes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `meal_types`
--

DROP TABLE IF EXISTS `meal_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `meal_types` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `meal_name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meal_types`
--

LOCK TABLES `meal_types` WRITE;
/*!40000 ALTER TABLE `meal_types` DISABLE KEYS */;
INSERT INTO `meal_types` VALUES (1,'Затрак'),(2,'Обед'),(3,'Ужин');
/*!40000 ALTER TABLE `meal_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menu`
--

DROP TABLE IF EXISTS `menu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menu` (
  `Menu_ID` int NOT NULL AUTO_INCREMENT,
  `User_id` bigint NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  PRIMARY KEY (`Menu_ID`),
  KEY `User_id_idx` (`User_id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu`
--

LOCK TABLES `menu` WRITE;
/*!40000 ALTER TABLE `menu` DISABLE KEYS */;
INSERT INTO `menu` VALUES (1,1,'Язва желудка и двенадцатиперстной кишки'),(2,2,'Гастрит'),(3,3,'Запор'),(4,4,'Болезнь кишечника с диареей'),(5,5,'Заболевания желчных путей и печени'),(6,6,'Мочекаменная болезнь, подагра'),(7,7,'Хронический и острый нефрит'),(8,8,'Ожирение'),(9,9,'Сахарный диабет'),(10,10,'Заболевания сердечно-сосудистой системы'),(11,11,'Туберкулез'),(12,12,'Заболевания нервной системы'),(13,13,'Острые инфекционные заболевания'),(14,14,'Болезнь почек с отхождением камней из фосфатов'),(15,15,'Заболевания, не требующие особых диет'),(16,16,'Вы абсолютно здоровы');
/*!40000 ALTER TABLE `menu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menu_dishes`
--

DROP TABLE IF EXISTS `menu_dishes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menu_dishes` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Tables_id` int NOT NULL,
  `Dishes_id` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `Dishes_id_idx` (`Dishes_id`),
  KEY `Table_id_idx` (`Tables_id`),
  CONSTRAINT `Dishes_id` FOREIGN KEY (`Dishes_id`) REFERENCES `dishes` (`Dishes_ID`),
  CONSTRAINT `tab_id` FOREIGN KEY (`Tables_id`) REFERENCES `tables` (`Table_number`)
) ENGINE=InnoDB AUTO_INCREMENT=105 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu_dishes`
--

LOCK TABLES `menu_dishes` WRITE;
/*!40000 ALTER TABLE `menu_dishes` DISABLE KEYS */;
INSERT INTO `menu_dishes` VALUES (4,1,1),(5,1,2),(6,2,1),(7,2,3),(8,3,2),(9,3,3),(10,16,1),(11,16,2),(12,16,3),(15,1,7),(16,2,7),(17,3,7),(18,5,7),(19,6,7),(20,8,7),(21,10,7),(22,11,7),(23,12,7),(24,16,7),(25,14,7),(26,15,7),(27,9,7),(41,16,9),(42,1,9),(43,2,9),(44,3,9),(45,5,9),(46,7,9),(47,9,9),(48,10,9),(49,12,9),(50,13,9),(58,16,13),(59,11,13),(60,6,13),(61,3,13),(62,16,14),(63,11,14),(64,6,14),(65,3,14),(66,8,14),(67,4,14),(68,16,15),(69,11,15),(70,6,15),(71,3,15),(72,8,15),(73,4,15),(74,1,15),(98,16,18),(99,11,18),(100,4,18),(101,5,18),(102,9,18),(103,13,18),(104,14,18);
/*!40000 ALTER TABLE `menu_dishes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tables`
--

DROP TABLE IF EXISTS `tables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tables` (
  `Table_number` int NOT NULL AUTO_INCREMENT,
  `one` tinyint NOT NULL DEFAULT '1',
  `two` tinyint NOT NULL DEFAULT '1',
  `three` tinyint NOT NULL DEFAULT '1',
  `four` tinyint NOT NULL DEFAULT '1',
  `five` tinyint NOT NULL DEFAULT '1',
  `six` tinyint NOT NULL DEFAULT '1',
  `seven` tinyint NOT NULL DEFAULT '1',
  `eight` tinyint NOT NULL DEFAULT '1',
  `nine` tinyint NOT NULL DEFAULT '1',
  `ten` tinyint NOT NULL DEFAULT '1',
  `eleven` tinyint NOT NULL DEFAULT '1',
  `twelve` tinyint NOT NULL DEFAULT '1',
  PRIMARY KEY (`Table_number`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tables`
--

LOCK TABLES `tables` WRITE;
/*!40000 ALTER TABLE `tables` DISABLE KEYS */;
INSERT INTO `tables` VALUES (1,1,0,0,0,1,1,1,1,0,1,1,1),(2,0,0,0,1,0,1,0,1,1,0,1,1),(3,0,0,1,1,1,1,1,1,1,1,0,1),(4,0,0,1,1,0,0,0,1,0,1,0,1),(5,1,0,1,1,1,0,0,1,0,1,1,1),(6,1,0,0,0,0,1,1,0,1,1,1,1),(7,1,0,0,0,0,1,0,0,1,0,1,1),(8,0,0,0,1,1,1,1,1,1,0,0,0),(9,1,0,0,1,0,1,0,0,1,1,0,1),(10,1,0,0,1,0,1,0,0,1,1,0,1),(11,1,0,0,0,1,1,1,1,0,0,1,0),(12,1,0,1,0,1,1,0,1,1,0,0,0),(13,1,0,1,0,0,0,1,0,1,0,1,1),(14,0,0,0,1,1,0,1,1,1,0,0,1),(15,1,0,1,0,1,1,1,1,0,1,1,0),(16,1,1,1,1,1,1,1,1,1,1,1,1);
/*!40000 ALTER TABLE `tables` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `types_of_dishes`
--

DROP TABLE IF EXISTS `types_of_dishes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `types_of_dishes` (
  `Type_ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  PRIMARY KEY (`Type_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `types_of_dishes`
--

LOCK TABLES `types_of_dishes` WRITE;
/*!40000 ALTER TABLE `types_of_dishes` DISABLE KEYS */;
INSERT INTO `types_of_dishes` VALUES (1,'Овощное'),(2,'Мясное'),(3,'Молочное'),(4,'Соусы'),(5,'Грибное'),(6,'Острое'),(7,'Хлебобулочные изделия'),(8,'Кофе'),(9,'Сладкое'),(10,'Копчености/ колбасные изделия'),(11,'Маринованная продукция'),(12,'Бобовые');
/*!40000 ALTER TABLE `types_of_dishes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `types_of_tables`
--

DROP TABLE IF EXISTS `types_of_tables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `types_of_tables` (
  `Type_table_ID` int NOT NULL AUTO_INCREMENT,
  `Menu_id` int NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  PRIMARY KEY (`Type_table_ID`),
  KEY `Menu_id_idx` (`Menu_id`),
  CONSTRAINT `Menu_id` FOREIGN KEY (`Menu_id`) REFERENCES `menu` (`Menu_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `types_of_tables`
--

LOCK TABLES `types_of_tables` WRITE;
/*!40000 ALTER TABLE `types_of_tables` DISABLE KEYS */;
INSERT INTO `types_of_tables` VALUES (20,1,'Первый'),(21,2,'Второй'),(22,3,'Третий'),(23,4,'Четвертый'),(24,5,'Пятый'),(25,6,'Шестой'),(26,7,'Седьмой'),(27,8,'Восьмой'),(28,9,'Девятый'),(29,10,'Десятый'),(30,11,'Одиннадцать'),(31,12,'Двенадцать'),(32,13,'Тринадцать'),(33,14,'Четырнадцать'),(34,15,'Пятнадцать'),(35,16,'Шестнадцать');
/*!40000 ALTER TABLE `types_of_tables` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `User_ID` bigint NOT NULL AUTO_INCREMENT,
  `Firstname` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `Surname` varchar(255) DEFAULT NULL,
  `Table_number_id` bigint NOT NULL DEFAULT '16',
  `Question_number` int NOT NULL,
  `Answer_variant_id` bigint NOT NULL,
  PRIMARY KEY (`User_ID`),
  UNIQUE KEY `User_ID_UNIQUE` (`User_ID`),
  KEY `Answer_variant_id_idx` (`Answer_variant_id`),
  KEY `Table_id_idx` (`Table_number_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5410944163 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (400854468,'MAN',NULL,16,0,400854468),(571254417,'Юлия',NULL,16,0,571254417),(575632196,'Simon','Vashchilov',16,0,575632196),(984814639,'Ann',NULL,16,8,984814639),(1038282804,'Sergei','Russkikh',16,0,1038282804),(1590207474,'Involuntar',NULL,16,0,1590207474),(5410944162,'Rikka',NULL,16,0,5410944162);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'nutriplan'
--

--
-- Dumping routines for database 'nutriplan'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-06-14 16:11:25
