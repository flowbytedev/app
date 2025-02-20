CREATE TABLE [dbo].[sales_price_current]
(
    [cdc_key] BIGINT NULL, 
    [company_id] NVARCHAR(10) NOT NULL, 
    [item_no] NVARCHAR(255) NOT NULL, 
    [variant_code] NVARCHAR(255) NOT NULL,
    [sales_code] NVARCHAR(255) NOT NULL, 
    [currency_code] NVARCHAR(10) NOT NULL, 
    [unit_of_measure] NVARCHAR(255) NOT NULL, 
    [promotion_code] NVARCHAR(255) NULL, 
    [original_price] DECIMAL(38, 20) NOT NULL, 
    [discount_price] DECIMAL(38, 20) NOT NULL, 
    [original_price_vat] DECIMAL(38, 20) NOT NULL, 
    [discount_price_vat] DECIMAL(38, 20) NOT NULL,
    
    
 
    CONSTRAINT [FK_sales_price_current_company] FOREIGN KEY ([company_id]) REFERENCES [company]([id]), 
    CONSTRAINT [FK_sales_price_current_item] FOREIGN KEY ([company_id],[item_no]) REFERENCES [item]([company_id],[item_no]), 
    CONSTRAINT [FK_sales_price_current_currency] FOREIGN KEY ([currency_code]) REFERENCES [currency]([code]), 
    CONSTRAINT [PK_sales_price_current] PRIMARY KEY NONCLUSTERED ([company_id],  [item_no], [variant_code], [sales_code], [currency_code],[unit_of_measure]) 
)
ON [FLOWBYTE_SALES];

GO

CREATE CLUSTERED INDEX [IX_sales_price_current_item_no_variant_code] ON [dbo].[sales_price_current] ([item_no],[variant_code])
ON [FLOWBYTE_SALES];

GO


CREATE INDEX [IX_sales_price_current_company_id] ON [dbo].[sales_price_current] ([company_id])

GO

CREATE INDEX [IX_sales_price_current_sales_code] ON [dbo].[sales_price_current] ([sales_code])
