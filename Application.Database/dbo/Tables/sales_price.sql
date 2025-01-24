CREATE TABLE [dbo].[sales_price]
(
    [company_id] NVARCHAR(10) NOT NULL, 
    [item_no] NVARCHAR(255) NOT NULL, 
    [variant_code] NVARCHAR(255) NOT NULL, 
    [date] DATE NOT NULL, 
    [price] DECIMAL(18, 2) NOT NULL, 
    [currency_code] NVARCHAR(10) NOT NULL, 
    
    CONSTRAINT [PK_sales_price] PRIMARY KEY ([company_id], [item_no], [date]), 
    CONSTRAINT [FK_sales_price_company] FOREIGN KEY ([company_id]) REFERENCES [company]([id]), 
    CONSTRAINT [FK_sales_price_item] FOREIGN KEY ([company_id],[item_no]) REFERENCES [item]([company_id],[item_no]), 
)

GO

CREATE INDEX [IX_sales_price_company_id_item_no] ON [dbo].[sales_price] ([company_id],[item_no])

GO


CREATE INDEX [IX_sales_price_valid_from] ON [dbo].[sales_price] ([valid_from])
