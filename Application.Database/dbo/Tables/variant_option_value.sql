CREATE TABLE [dbo].[variant_option_value]
(
    [cdc_key] BIGINT NULL,
    [company_id] NVARCHAR(10) NOT NULL, 
    [item_no] NVARCHAR(255) NOT NULL, 
    [status] INT NULL, 
    [description] NVARCHAR(255) NULL,
    [option_name] NVARCHAR(255) NOT NULL, 
    [value] NVARCHAR(255) NOT NULL, 
     
     
    CONSTRAINT [PK_variant_option_value] PRIMARY KEY ([company_id], [item_no], [option_name]), 
    CONSTRAINT [FK_variant_option_value_company] FOREIGN KEY ([company_id]) REFERENCES [company]([id]), 
    CONSTRAINT [FK_variant_option_value_item] FOREIGN KEY ([company_id],[item_no]) REFERENCES [item]([company_id],[item_no]), 
    CONSTRAINT [FK_variant_option_value_option_name] FOREIGN KEY ([company_id],[item_no],[option_name]) REFERENCES [variant_option]([company_id],[item_no],[name]) 
    
)
ON [FLOWBYTE_DIM];
GO

CREATE INDEX [IX_variant_option_value_company_id_item] ON [dbo].[variant_option_value] ([company_id],[item_no])
ON [FLOWBYTE_DIM];

GO

CREATE INDEX [IX_variant_option_value_option_name] ON [dbo].[variant_option_value] ([option_name])
ON [FLOWBYTE_DIM];
