CREATE DATABASE IF NOT EXISTS Malshinon;

USE Malshinon;

CREATE TABLE IF NOT EXISTS People (
    `ID` INT PRIMARY KEY AUTO_INCREMENT,
    `First_Name` VARCHAR(20) UNIQUE NOT NULL,
    `Last_Name` VARCHAR(20) UNIQUE NOT NULL,
    `Secret_Code` VARCHAR(20) UNIQUE NOT NULL,
    `Type` ENUM ('reporter','target','both','potential_agent') DEFAULT 'reporter',
    `Num_Reports` INT DEFAULT 0,
    `Num_Mentions` INT DEFAULT 0
);

CREATE TABLE IF NOT EXISTS IntelReports(
    `ID` INT PRIMARY KEY AUTO_INCREMENT,
    `Reporter_Id` INT,
    FOREIGN KEY (`Reporter_Id`) REFERENCES `People`(`ID`),
    `Target_Id` INT,
    FOREIGN KEY (`Target_Id`) REFERENCES `People`(`ID`),
    `Text` TEXT,
    `Timestamp` DATETIME DEFAULT NOW()
    
);

