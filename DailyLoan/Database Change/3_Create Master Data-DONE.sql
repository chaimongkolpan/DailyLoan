USE [DailyLoan]
GO


INSERT INTO [UserAccess] (ID,UserAccess) VALUES (1,'ผู้ดูแลระบบ'),(2,'เสมียน'),(3,'คนตรวจ'),(4,'คนเก็บ');
INSERT INTO [Status_User] (ID,Status) VALUES (1,'ใช้งาน'),(2,'ไม่ใช้งาน');
INSERT INTO [Status_House] (ID,Status) VALUES (1,'ใช้งาน'),(2,'ไม่ใช้งาน');
INSERT INTO [Status_CustomerLine] (ID,Status) VALUES (1,'ใช้งาน'),(2,'ไม่ใช้งาน');
INSERT INTO [Status_Customer] (ID,Status) VALUES (1,'ดี'),(2,'แย่'),(3,'เสีย');
INSERT INTO [Status_Contract] (ID,Status) VALUES (1,'รอยืนยัน'),(2,'แจ้งเตือน'),(3,'ปกติ'),(4,'รอตรวจสอบผิดชำระ'),(5,'ตรวจสอบ'),(6,'เสีย'),(7,'ตาย'),(8,'ปิดสัญญา');
INSERT INTO [Status_Request] (ID,Status) VALUES (1,'รออนุมัติ'),(2,'อนุมัติ'),(3,'ไม่อนุมัติ');
INSERT INTO [Status_Notification] (ID,Status) VALUES (2,'แจ้งเตือน'),(4,'รอตรวจสอบผิดชำระ'),(5,'ตรวจสอบ');
INSERT INTO [Status_Transaction] (ID,Status) VALUES (1,'ปกติ'),(2,'จ่ายไม่ครบ'),(3,'ไม่จ่าย'),(4,'ตัด');

INSERT INTO [RequestType] (ID,Name) VALUES (1,'เปิดกู้'),(2,'ตัด'),(3,'ตัดลด'),(4,'ตัดเพิ่ม');
INSERT INTO [NotificationType] (ID,Name) VALUES (1,'ผิดนัดชำระ'),(2,'เปิดกู้'),(3,'ตัด'),(4,'อื่นๆ');
INSERT INTO [TransactionType] (ID,Name) VALUES (1,'เก็บได้'),(2,'ค่าหัว'),(3,'จ่ายไป');

GO


INSERT INTO [Config] ([HouseID],[Name],[Value],[CreateBy],[CreateDate]) 
	VALUES 
		(1,'CustomerRate','90',1,'2021-12-01'),
		(1,'AgentRate','5',1,'2021-12-01'),
		(1,'HouseRate','5',1,'2021-12-01'),
		(1,'MinCutDay','6',1,'2021-12-01'),
		(1,'IncCutCriteria','4',1,'2021-12-01'),
		(1,'DecCutCriteria','4',1,'2021-12-01'),
		(1,'DecCutPercen','80',1,'2021-12-01'),
		(1,'SpecialRateCriteria','3',1,'2021-12-01'),
		(1,'TotalProfit','20',1,'2021-12-01'),
		(1,'NotPayAlert','3',1,'2021-12-01'),
		(1,'PartialPayAlert','3',1,'2021-12-01');

INSERT INTO [House] ([ID],[HouseName],[Region],[Province],[Status],[Remark],[CreateBy],[CreateDate]) 
	VALUES (1,'MainHouse','-','-',1,'',1,'2021-12-01');


INSERT INTO [User] ([ID],[Username],[Password],[UserAccess],[Firstname],[Lastname],[Nickname],[Phone1],[Phone2]
      ,[Status] ,[HouseID],[CreateBy],[CreateDate]) VALUES 
	  (1,'root','BrKjilpPyNU3vYxqm8KrCg==',1,'Super','Admin','-','-','-',
	  1,1,1,'2021-12-01');
GO