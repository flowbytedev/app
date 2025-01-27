CREATE TABLE [dbo].[inventory]
(
	[company_id] NVARCHAR(10) NOT NULL, 
    [item_no] NVARCHAR(255) NOT NULL, 
    [variant_no] NVARCHAR(255) NOT NULL, 
    [warehouse_code] NVARCHAR(255) NOT NULL, 
    [date] DATE NOT NULL,
    [unit] NVARCHAR(50) NOT NULL,
    [amount] INT NOT NULL, 
    [inventory_type] NVARCHAR(255) NOT NULL, 
    [batch_no] NVARCHAR(255) NOT NULL, 
    [source_reference] NVARCHAR(255) NOT NULL, 
    [expiration_date] DATE NOT NULL, 

    CONSTRAINT [PK_inventory] PRIMARY KEY ([company_id], [item_no], [variant_no], [warehouse_code], [date]), 

    CONSTRAINT [FK_inventory_company] FOREIGN KEY ([company_id]) REFERENCES [company]([id]),
    CONSTRAINT [FK_inventory_company_id_item_no] FOREIGN KEY ([company_id],[item_no]) REFERENCES [item]([company_id],[item_no]), 
    CONSTRAINT [FK_inventory_warehouse] FOREIGN KEY ([company_id],[warehouse_code]) REFERENCES [warehouse]([company_id],[code]), 
    
    
     
)
ON [FLOWBYTE_DIM];

GO

CREATE INDEX [IX_inventory_item_no_variant_no] ON [dbo].[inventory] ([item_no],[variant_no])
ON [FLOWBYTE_DIM];

GO

CREATE INDEX [IX_inventory_warehouse] ON [dbo].[inventory] ([warehouse_code])
ON [FLOWBYTE_DIM];

