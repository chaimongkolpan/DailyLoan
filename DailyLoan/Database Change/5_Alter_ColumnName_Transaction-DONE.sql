/*
   Thursday, November 25, 202100:01:24
   User: 
   Server: BBOAT-PC\SQLEXPRESS
   Database: DailyLoan2
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
EXECUTE sp_rename N'dbo.[Transaction].AgentID', N'Tmp_CustomerLineID', 'COLUMN' 
GO
EXECUTE sp_rename N'dbo.[Transaction].Tmp_CustomerLineID', N'CustomerLineID', 'COLUMN' 
GO
ALTER TABLE dbo.[Transaction] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.[Transaction]', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.[Transaction]', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.[Transaction]', 'Object', 'CONTROL') as Contr_Per 