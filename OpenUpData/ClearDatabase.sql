-- Set proper options
SET QUOTED_IDENTIFIER ON
GO

-- Disable foreign key constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
GO

-- Delete data from all tables except __EFMigrationsHistory
DECLARE @sql NVARCHAR(MAX) = ''
SELECT @sql += 'DELETE FROM [' + SCHEMA_NAME(schema_id) + '].[' + name + '];'
FROM sys.tables
WHERE name != '__EFMigrationsHistory'
EXEC sp_executesql @sql
GO

-- Reset identity columns
DECLARE @sql NVARCHAR(MAX) = ''
SELECT @sql += 'IF EXISTS (SELECT 1 FROM sys.identity_columns WHERE object_id = OBJECT_ID(''' + 
               SCHEMA_NAME(schema_id) + '.' + name + '''))
               DBCC CHECKIDENT(''' + SCHEMA_NAME(schema_id) + '.' + name + ''', RESEED, 0);'
FROM sys.tables
WHERE name != '__EFMigrationsHistory'
EXEC sp_executesql @sql
GO

-- Re-enable foreign key constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
GO