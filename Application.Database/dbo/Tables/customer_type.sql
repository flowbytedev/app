CREATE TABLE [dbo].[customer_type]
(
    [code] NVARCHAR(255) NOT NULL, 
    [type] NVARCHAR(255) NOT NULL, 
    [description] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_customer_type] PRIMARY KEY ([code]) 
)
