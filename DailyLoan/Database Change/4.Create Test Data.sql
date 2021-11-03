USE [DailyLoan]
GO

INSERT INTO [User] (Username,Password,UserAccess,Firstname,Lastname,Nickname,Phone1,Phone2,Status,HouseID,CreateBy,CreateDate)
VALUES ('root','-',1,'','','','','',1,0,0,'2021-11-03 18:00:00');

INSERT INTO [House] (HouseName,Province,District,SubDistrict,Address,Status,Remark,CreateBy,CreateDate)
VALUES ('test','กรุงเทพมหานคร','ดอนเมือง','ดอนเมือง','123',1,'-',1,'2021-11-03 18:00:00');


INSERT INTO [User] (Username,Password,UserAccess,Firstname,Lastname,Nickname,Phone1,Phone2,Status,HouseID,CreateBy,CreateDate)
VALUES ('admin','-',2,'เสมียน','1','เสมียน1','-','-',1,1,1,'2021-11-03 18:00:00'),
		('auditor','-',3,'คนตรวจ','1','ตรวจ1','-','-',1,1,1,'2021-11-03 18:00:00'),
		('ag1','-',4,'คนเก็บ1','-','เก็บ1','099999','-',1,1,1,'2021-11-03 18:00:00'),
		('ag2','-',4,'ผช','เก็บ1-1','ผช1-1','088888','-',1,1,1,'2021-11-03 18:00:00'),
		('ag3','-',4,'ผช','เก็บ1-2','ผช1-2','081111','01234',1,1,1,'2021-11-03 18:00:00'),
		('ag4','-',4,'คนเก็บ2','-','เก็บ2','082111','01234',1,1,1,'2021-11-03 18:00:00'),
		('ag5','-',4,'ผช','เก็บ2-1','ผช1','010000','-',1,1,1,'2021-11-03 18:00:00'),
		('ag6','-',4,'ผช','เก็บ2-2','ผช2','020000','-',1,1,1,'2021-11-03 18:00:00');

INSERT INTO [CustomerLine] (CustomerLineName,HouseID,Status,Remark,CreateBy,CreateDate)
VALUES ('สาย1',1,1,'-',1,'2021-11-03 18:00:00'),
		('สาย2',1,1,'-',1,'2021-11-03 18:00:00');

INSERT INTO [User_Permission] (UserID,CustomerLineID,CreateBy,CreateDate)
VALUES (4,1,1,'2021-11-03 18:00:00'),
		(5,1,1,'2021-11-03 18:00:00'),
		(6,1,1,'2021-11-03 18:00:00'),
		(7,2,1,'2021-11-03 18:00:00'),
		(8,2,1,'2021-11-03 18:00:00'),
		(9,2,1,'2021-11-03 18:00:00');

INSERT INTO [Config] ([HouseID],[Name],[Value],[CreateBy],[CreateDate]) 
VALUES (1,'CustomerRate','90',1,'2021-11-03 18:00:00'),
(1,'AgentRate','5',1,'2021-11-03 18:00:00'),
(1,'HouseRate','5',1,'2021-11-03 18:00:00'),
(1,'MinCut(Day)','6',1,'2021-11-03 18:00:00'),
(1,'IncCutCriteria','3',1,'2021-11-03 18:00:00'),
(1,'DecCutCriteria','3',1,'2021-11-03 18:00:00'),
(1,'DecCutPercen','80',1,'2021-11-03 18:00:00'),
(1,'SpecialRateCriteria','3',1,'2021-11-03 18:00:00'),
(1,'TotalProfit','20',1,'2021-11-03 18:00:00'),
(1,'NotPayAlert','3',1,'2021-11-03 18:00:00'),
(1,'PartialPayAlert','3',1,'2021-11-03 18:00:00');