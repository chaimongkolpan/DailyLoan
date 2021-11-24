USE [DailyLoan]
GO

INSERT INTO [UserAccess] (ID,UserAccess) VALUES (1,'ผู้ดูแลระบบ'),(2,'เสมียน'),(3,'คนตรวจ'),(4,'คนเก็บ');
INSERT INTO [Status_User] (ID,Status) VALUES (1,'ใช้งาน'),(2,'ไม่ใช้งาน');
INSERT INTO [Status_House] (ID,Status) VALUES (1,'ใช้งาน'),(2,'ไม่ใช้งาน');
INSERT INTO [Status_CustomerLine] (ID,Status) VALUES (1,'ใช้งาน'),(2,'ไม่ใช้งาน');
INSERT INTO [Status_Customer] (ID,Status) VALUES (1,'ดี'),(2,'แย่'),(3,'เสีย');
INSERT INTO [Status_Contract] (ID,Status) VALUES (1,'รอยืนยัน'),(2,'ปกติ'),(3,'ปิดสัญญา'),(4,'แจ้งเตือน'),(5,'รอตรวจสอบผิดชำระ'),(6,'ตรวจสอบ'),(7,'เสีย');
INSERT INTO [Status_Request] (ID,Status) VALUES (1,'รออนุมัติ'),(2,'อนุมัติ'),(3,'ไม่อนุมัติ');
INSERT INTO [Status_Notification] (ID,Status) VALUES (1,'แจ้งเตือน'),(2,'รอตรวจสอบ'),(3,'หยุดชั่วคราว'),(4,'ตรวจสอบแล้ว');
INSERT INTO [Status_Transaction] (ID,Status) VALUES (1,'ปกติ'),(2,'จ่ายไม่ครบ'),(3,'ไม่จ่าย'),(4,'ตัด');

INSERT INTO [RequestType] (ID,Name) VALUES (1,'เปิดกู้'),(2,'ตัด'),(3,'ตัดลด'),(4,'ตัดเพิ่ม');
INSERT INTO [NotificationType] (ID,Name) VALUES (1,'ผิดนัดชำระ'),(2,'เปิดกู้'),(3,'ตัด'),(4,'อื่นๆ');
INSERT INTO [TransactionType] (ID,Name) VALUES (1,'ชำระ'),(2,'ค่าหัว');