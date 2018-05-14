DELETE FROM [JoinSamples].[Employee]
DELETE FROM [JoinSamples].[Department]

INSERT INTO [JoinSamples].[Department] ([Id], [DepartmentName]) 
SELECT 31, N'Sales' UNION 
SELECT 33, N'Engineering' UNION 
SELECT 34, N'Clerical' UNION 
SELECT 35, N'Marketing'

INSERT INTO [JoinSamples].[Employee] ([Id], [LastName], [DepartmentID]) VALUES (1, N'Rafferty', 31)
INSERT INTO [JoinSamples].[Employee] ([Id], [LastName], [DepartmentID]) VALUES (2, N'Jones', 33)
INSERT INTO [JoinSamples].[Employee] ([Id], [LastName], [DepartmentID]) VALUES (3, N'Heisenberg', 33)
INSERT INTO [JoinSamples].[Employee] ([Id], [LastName], [DepartmentID]) VALUES (4, N'Robinson', 34)
INSERT INTO [JoinSamples].[Employee] ([Id], [LastName], [DepartmentID]) VALUES (5, N'Smith', 34)
INSERT INTO [JoinSamples].[Employee] ([Id], [LastName], [DepartmentID]) VALUES (6, N'Williams', NULL)
GO

