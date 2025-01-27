CREATE TABLE [dbo].[customer_status]
(
    [code] NVARCHAR(255) NOT NULL, 
    [type] NVARCHAR(255) NOT NULL, 
    [description] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_customer_status] PRIMARY KEY ([code]),
    
)
