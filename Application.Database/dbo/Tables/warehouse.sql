﻿CREATE TABLE [dbo].[warehouse]
(
	[company_id] NVARCHAR(10) NOT NULL, 
    [code] NVARCHAR(255) NOT NULL, 
    [name] NVARCHAR(255) NOT NULL, 
    [address] NVARCHAR(255) NOT NULL, 
    [city] NVARCHAR(255) NULL, 
    [postal_code] NVARCHAR(255) NULL, 
    [phone_no] NVARCHAR(255) NULL, 
    [latitude] DECIMAL(18, 6) NULL, 
    [longitude] DECIMAL(18, 6) NULL, 
    [email] NVARCHAR(255) NULL, 
    CONSTRAINT [PK_warehouse] PRIMARY KEY ([company_id], [code]), 
    CONSTRAINT [FK_warehouse_company_id] FOREIGN KEY ([company_id]) REFERENCES [company]([id]) 
)

GO

CREATE INDEX [IX_warehouse_name] ON [dbo].[warehouse] ([name])

GO

CREATE INDEX [IX_warehouse_city] ON [dbo].[warehouse] ([city])