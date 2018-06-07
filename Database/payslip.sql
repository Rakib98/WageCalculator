DROP DATABASE IF EXISTS payslip_db;
CREATE DATABASE payslip_db;
USE payslip_db;

CREATE TABLE payslip_tbl (
		id INT PRIMARY KEY AUTO_INCREMENT,
		title VARCHAR(5),
		name VARCHAR(25) NOT NULL,
		last_name VARCHAR(25) NOT NULL,
		NI_num VARCHAR(9) NOT NULL,
		hours DOUBLE NOT NULL,
		rate DOUBLE NOT NULL,
		overtime DOUBLE NOT NULL,
		income_tax DOUBLE,
		ni_charge DOUBLE,
		gross DOUBLE NOT NULL,
		net DOUBLE NOT NULL,
		date_ DATE
)ENGINE=INNODB;

SELECT * FROM payslip_tbl;