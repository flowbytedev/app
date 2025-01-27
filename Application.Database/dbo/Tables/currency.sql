CREATE TABLE [dbo].[currency]
( 
    [code] NVARCHAR(10) NOT NULL, 
    [name] NVARCHAR(255) NOT NULL, 
    [description] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_currency] PRIMARY KEY ([code]), 
    
)
ON [FLOWBYTE_DIM];
