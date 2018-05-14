/*Patch 1.3*/
GO
use "Northwind"
GO
if exists (select * from sysobjects where id = object_id('dbo.Region') and sysstat & 0xf = 3)
	EXEC sp_rename 'Region', 'Regions';
GO