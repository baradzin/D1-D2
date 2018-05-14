-- Reference Data for AddressType 
MERGE INTO AddressType AS Target 
USING (VALUES 
  (1, N'Billing'), 
  (2, N'Home'), 
  (3, N'Main Office'), 
  (4, N'Primary'), 
  (5, N'Shipping'), 
  (6, N'Archive'), 
  (0, N'Undefined') 
) AS Source (AddressTypeID, Name) 
ON Target.AddressTypeID = Source.AddressTypeID 
-- update matched rows 
WHEN MATCHED THEN 
	UPDATE SET Name = Source.Name 
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (AddressTypeID, Name) 
	VALUES (AddressTypeID, Name) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
	DELETE;