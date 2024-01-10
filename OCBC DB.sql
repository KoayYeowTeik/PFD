/*Create Database OCBC 
GO */

Use OCBC  
GO 



if exists (select * from sysobjects  
  where id = object_id('dbo.Cards') and sysstat & 0xf = 3) 
  drop table dbo.Cards
GO 

if exists (select * from sysobjects  
  where id = object_id('dbo.ActivityHistory') and sysstat & 0xf = 3) 
  drop table dbo.ActivityHistory

GO 

if exists (select * from sysobjects  
  where id = object_id('dbo.TranscationHistory') and sysstat & 0xf = 3) 
  drop table dbo.TranscationHistory
GO 

if exists (select * from sysobjects  
  where id = object_id('dbo.SpecialNeeds') and sysstat & 0xf = 3) 
  drop table dbo.SpecialNeeds
GO 

if exists (select * from sysobjects  
  where id = object_id('dbo.Users') and sysstat & 0xf = 3) 
  drop table dbo.Users
GO 

CREATE TABLE dbo.Users --10 users 2 caretaker 4 special need 2 autistic linked to 2 caretaker
(
	userID int IDENTITY (1,1),
	userName varchar(255) NOT NULL,
	LoginID varchar (255) NOT NULL,
	[Password] varchar(255) NOT NULL,
	contactNumber varchar(20) NOT NULL,
	balance money NOT NULL DEFAULT(0),
	accountNumber char(10) NOT NULL,
	dob DateTime NOT NULL,
	limitDay money NULL,
	limitMonth money NULL,
	limitWeek money NULL,
	faceid varchar(255) NULL,
	digitalToken char(20) NULL,
	CONSTRAINT PK_USERS PRIMARY KEY NONCLUSTERED (userID)
)
GO


CREATE TABLE dbo.Cards -- 10 cards w 10 user
(
	cardID int IDENTITY (1,1),
	userID int NOT NULL,
	cardNumber char(16) NOT NULL ,
	cVV char(3) NOT NULL,
	expDate char(5) NOT NULL,
	cardType varchar(16) NOT NULL,
	billingAddress varchar(255) NOT NULL,
	CONSTRAINT PK_CardID PRIMARY KEY NONCLUSTERED (cardID),
	CONSTRAINT FK_CardID FOREIGN KEY (userID)
	REFERENCES dbo.Users(userID)
)
GO

CREATE TABLE dbo.ActivityHistory --dn do
(
	recordID int IDENTITY(1,1),
	activityTime datetime NOT NULL,
	[description] varchar(255) NOT NULL,
	userID int NOT NULL,
	CONSTRAINT PK_ActivityRecordID PRIMARY KEY NONCLUSTERED (recordID),
	CONSTRAINT FK_ActivityUserID FOREIGN KEY (userID)
	REFERENCES dbo.Users(userID)
)
GO

CREATE TABLE dbo.TransactionHistory --dn do
(
	recordID int IDENTITY (1,1),
	transactionTime datetime NOT NULL,
	[description] varchar(255) NULL,
	senderID int NOT NULL,
	receiverID int NOT NULL,
	amount money NOT NULL,
	[status] varchar(10) NOT NULL,
	category varchar(50) NOT NULL,
	CONSTRAINT PK_TransactionRecordID PRIMARY KEY NONCLUSTERED (recordID),
	CONSTRAINT FK_TransactionSenderID FOREIGN KEY (senderID) REFERENCES dbo.Users(userID),
	CONSTRAINT FK_TransactionRecordID FOREIGN KEY (ReceiverID) REFERENCES dbo.Users(userID),
)
GO

CREATE TABLE dbo.SpecialNeeds -- 4 special need 2 autistic linked to 2 caretaker other 2 is learning disabled
(
	recordID int IDENTITY (1,1),
	userID int NOT NULL,
	helperID int NULL,
	[type] varchar(255) NOT NULL,
	CONSTRAINT PK_SNRecordID PRIMARY KEY NONCLUSTERED (recordID),
	CONSTRAINT FK_NeedsUserID FOREIGN KEY (userID) REFERENCES dbo.Users(userID),
	CONSTRAINT FK_HelperUserID FOREIGN KEY (helperID) REFERENCES dbo.Users(userID),
)
GO

CREATE TABLE dbo.bankAccount -- bankaccounts
(
	accountID int IDENTITY (1,1),
	accountNumber char(10) NOT NULL,
	balance money NOT NULL DEFAULT(0),
	userID int NOT NULL,
	CONSTRAINT PK_bankAccount PRIMARY KEY (accountID),
	CONSTRAINT FK_bankAccount_userID FOREIGN KEY(userID) REFERENCES dbo.Users(userID)
)
GO

SET IDENTITY_INSERT [dbo].Users ON 
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (1,'LordConcubine','LORDCUBE','P4WCI0pXRq','34738478','3897956421',7332,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (2,'Dafacz','Dafacz21','s%4&LbF1nP','89637072','0719917146',3849,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (3,'Canon','Kenan','lNr8Q33O9y','88627618','6345577660',298,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (4,'FMM','BiDe','!64UU8s)pH','97436972','1564595295',29,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (5,'Liwei','Liwei','AzPJzgImma','90951519','4448235949',23762,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (6,'Legend27','WenBin','vEgoH$m4zI','91555892','7817583794',232893,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (7,'Sunrise_yay:)','TinaGao','w2s2J0SYi1','95811521','8659495276',252,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (8,'Sunsetyay:)','GaoBorou','oEJEyQH055','89350350','0661348101',2323,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (9,'Kh','Kehui','KxghUauopZ','91026716','4855539472',2132,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
INSERT [dbo].Users ([userID],[userName],[LoginID],[Password],[contactNumber],[accountNumber],[balance],[dob],[limitDay],[limitMonth],[limitWeek],[faceid],[digitalToken]) VALUES (10,'ikheu','IKEUQT','zH=N/IDT3G','85982572','9168436641',1,'3/12/2005',NULL,NULL,NULL,NULL,NULL)
SET IDENTITY_INSERT [dbo].Users OFF

SET IDENTITY_INSERT [dbo].Cards ON
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (1,1,'9621556457832111',242,'12/26','Debit','65A Jalan Tenteram 01-12 Saint Michael"s Estate Flatted Factory')
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (2,2,'3137188686788325',272,'12/27','Debit','Little India Shop Houses 27 Dalhousie Lane')
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (3,3,'9317182058911662',829,'12/25','Debit','252 North Bridge Road #B1-51 Raffles City Shopping Centre')
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (4,4,'1740237742262437',838,'12/30','Credit','271 Bukit Timah Road #04-03 Blamoral Plaza')
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (5,5,'0312642041584063',283,'12/25','Debit','252 North Bridge Road #B1-51 Raffles City Shopping Centre')
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (6,6,'0971153462978911',919,'12/24','Debit','2 International Business Park TOWER 1 #11-06 The Strategy')
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (7,7,'8907027288859746',211,'12/23','Credit','Blk 502, Jelapang Road, ,02-386')
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (8,8,'3518816483607050',193,'12/30','Credit','10 Ubi Crescent Ubi Techpark')
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (9,9,'1226812133283325',293,'12/32','Debit','1014 GEYLANG EAST AVENUE 3 03-194')
INSERT INTO [dbo].Cards ([cardID],[userID],[cardNumber],[cVV],[expDate],[cardType],[billingAddress]) VALUES (10,10,'6608302549305981',231,'12/29','Credit','10 Anson Rd #35-06')
SET IDENTITY_INSERT [dbo].Cards OFF

SET IDENTITY_INSERT [dbo].ActivityHistory ON
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (1,'2023-10-01 13:23:44','User has logged into the system',1)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (2,'2023-10-01 13:24:24','User entered the transfer page',1)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description] ,[userID]) VALUES (3,'2023-10-01 13:25:21','User transfered 293 to another user',1)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (4,'2023-10-01 13:26:01','User exited the system',1)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (5,'2023-10-01 15:29:19','User has logged into the system',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (6,'2023-10-01 15:29:20','User has signed up into the system',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (7,'2023-10-01 15:31:12','User played the tutorial',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (8,'2023-10-01 15:30:12','User checked estatement for month September',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (9,'2023-10-01 15:35:14','User check notifications',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (10,'2023-10-01 15:36:20','User exited the system',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (11,'2023-10-02 09:13:52','User has logged into the system',3)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (12,'2023-10-01 15:35:23','User exited the system',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (13,'2023-10-02 09:12:24','User has logged into the system',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (14,'2023-10-02 09:14:53','User viewed user 9 activities',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (15,'2023-10-02 09:15:21','User exited the systesm',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (16,'2023-10-02 09:15:41','User has logged into the system',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (17,'2023-10-02 09:15:12','User exited the system',3)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (18,'2023-10-02 09:23:23','User entered the transfer page',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (19,'2023-10-02 09:23:24','User played the tutorial',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (20,'2023-10-02 09:30:23','User awaiting a transfer of 10000 to another user',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (21,'2023-10-02 09:32:28','User has logged into the system',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (22,'2023-10-02 09:33:29','User approved another user"s transfer request',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (23,'2023-10-02 09:33:30','User transfered 1000 to another user',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (24,'2023-10-30 09:12:29','User has signed up into the system',10)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (25,'2023-10-30 09:13:23','User played the tutorial',10)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (26,'2023-10-30 09:15:23','User has made a payment of 1 to another user',10)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (27,'2023-10-30 09:16:00','User exited the system',10)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (28,'2023-10-02 09:34:00','User entered the homepage',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (29,'2023-10-02 09:35:00','User exited the system',9)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (30,'2023-10-02 09:34:00','User exited the system',2)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (31,'2023-10-31 09:23:00','User has logged into the system',3)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description],[userID]) VALUES (32,'2023-10-31 09:24:24','User entered the transfer page',3)
INSERT INTO [dbo].ActivityHistory ([recordID],[activityTime],[description] ,[userID]) VALUES (33,'2023-10-31 09:25:21','User transfered 500 failed to another user',3)
SET IDENTITY_INSERT [dbo].ActivityHistory OFF

SET IDENTITY_INSERT [dbo].TransactionHistory ON

INSERT INTO [dbo].TransactionHistory ([recordID],[transactionTime],[description],[senderID],[receiverID],[amount],[status],[category]) 
VALUES 
    (1,'2022-09-15 13:25:21',NULL,1,8,293,1,'Transfers'),
    (2,'2022-09-16 09:33:30','Paying rent',9,2,10000,1,'Transfers'),
    (3,'2022-09-14 23:23:12','Owe',3,2,500,3,'Food'),
    (4,'2022-11-05 08:45:00','Shopping',1,4,150,1,'Bills'),
    (5,'2022-11-10 16:20:45',NULL,7,1,75,1,'Transfers'),
    (6,'2022-11-15 12:30:10','Groceries',3,9,200,1,'Food'),
    (7,'2022-11-20 14:55:30',NULL,5,2,300,1,'Transfers'),
    (8,'2022-11-25 09:10:15','Utilities',10,6,120,1,'Bills'),
    (9,'2022-11-30 18:40:22','Dinner',8,1,50,1,'Food'),
    (10,'2022-12-05 11:05:55',NULL,2,10,180,1,'Transfers'),
    (11,'2022-12-10 09:30:00','Entertainment',4,8,40,1,'Bills'),
    (12,'2022-12-15 14:20:45',NULL,9,3,120,1,'Transfers'),
    (13,'2022-12-20 12:30:10','Example',1,5,60,1,'Bills'),
    (14,'2022-12-25 08:55:30',NULL,6,10,90,1,'Transfers'),
    (15,'2022-12-30 16:10:15','Shopping',2,7,75,1,'Bills'),
    (16,'2023-01-05 11:30:00','Rent Payment',10,1,1000,1,'Bills'),
    (17,'2023-01-10 14:20:45',NULL,3,9,150,1,'Transfers'),
    (18,'2023-01-15 12:30:10','Groceries',8,2,200,1,'Food'),
    (19,'2023-01-20 08:55:30',NULL,7,4,120,1,'Transfers'),
    (20,'2023-01-25 16:10:15','Utilities',5,10,80,1,'Bills'),
    (21,'2023-10-10 09:30:00','Shopping',4,8,40,1,'Bills'),
    (22,'2023-10-15 14:20:45',NULL,9,3,120,1,'Transfers'),
    (23,'2023-10-20 12:30:10','Example',1,5,60,1,'Bills'),
    (24,'2023-10-25 08:55:30',NULL,6,10,90,1,'Transfers'),
    (25,'2023-10-30 16:10:15','Rent Payment',2,7,75,1,'Bills'),
    (26,'2023-11-05 11:30:00','Utilities',10,1,100,1,'Bills'),
    (27,'2023-11-10 14:20:45',NULL,3,9,150,1,'Transfers'),
    (28,'2023-11-15 12:30:10','Groceries',8,2,200,1,'Food'),
    (29,'2023-11-20 08:55:30',NULL,7,4,120,1,'Transfers'),
    (30,'2023-11-25 16:10:15','Dinner',5,10,80,1,'Food');

SET IDENTITY_INSERT [dbo].TransactionHistory OFF

SET IDENTITY_INSERT [dbo].SpecialNeeds ON
INSERT [dbo].SpecialNeeds ([recordID],[userID],[helperID],[type]) VALUES (1,9,2,'ADD')
INSERT [dbo].SpecialNeeds ([recordID],[userID],[helperID],[type]) VALUES (2,10,NULL,'LD')
SET IDENTITY_INSERT [dbo].SpecialNeeds OFF

SET IDENTITY_INSERT [dbo].SpecialNeeds ON
INSERT [dbo].bankAccount ([accountNumber], [balance], [userID])VALUES ('5425123456', 500.00, 1);
INSERT [dbo].bankAccount ([accountNumber], [balance], [userID])VALUES ('5425123456', 7500.00, 1);
INSERT [dbo].bankAccount ([accountNumber], [balance], [userID])VALUES ('5425123456', 12500.00, 2);
SET IDENTITY_INSERT [dbo].SpecialNeeds OFF
