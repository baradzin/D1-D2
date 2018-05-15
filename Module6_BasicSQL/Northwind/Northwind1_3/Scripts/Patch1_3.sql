USE "Northwind"
GO

if exists (select * from sysobjects where id = object_id('dbo.Region') and sysstat & 0xf = 3)
BEGIN
	BEGIN TRAN

	-- Drop Foreign Key FK_Territories_Region from Territories
	ALTER TABLE [dbo].[Territories] DROP CONSTRAINT [FK_Territories_Region]
	

	IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
	
	EXEC sp_rename N'[dbo].[Region]', N'Regions'
	

	IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
	
	-- Create Foreign Key FK_Territories_Region on Territories
	ALTER TABLE [dbo].[Territories]
		WITH CHECK
		ADD CONSTRAINT [FK_Territories_Region]
		FOREIGN KEY ([RegionID]) REFERENCES [dbo].[Regions] ([RegionID])
	ALTER TABLE [dbo].[Territories]
		CHECK CONSTRAINT [FK_Territories_Region]

	

	IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END
	

	IF @@TRANCOUNT>0
		COMMIT

	SET NOEXEC OFF
END