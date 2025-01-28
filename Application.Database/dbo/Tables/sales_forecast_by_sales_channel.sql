CREATE TABLE [dbo].[sales_forecast_by_store]
(
    [company_id] NVARCHAR(10) NOT NULL,
	[date] DATETIME NOT NULL, 
    [sales_channel_code] NVARCHAR(255) NOT NULL, 
    [net_amount_acy] DECIMAL(38, 20) NOT NULL, 
    [net_amount_acy_upper] DECIMAL(38, 20) NOT NULL, 
    [net_amount_acy_lower] DECIMAL(38, 20) NOT NULL, 
    CONSTRAINT [PK_sales_forecast_by_store] PRIMARY KEY NONCLUSTERED ([company_id], [date], [sales_channel_code]), 
    CONSTRAINT [FK_sales_forecast_by_store_store] FOREIGN KEY ([company_id], [sales_channel_code]) REFERENCES [sales_channel]([company_id], [code]) 
     
)
ON [FLOWBYTE_SALES];

GO

CREATE CLUSTERED INDEX [IX_sales_forecast_by_store_date] ON [dbo].[sales_forecast_by_store] ([date]) 
ON [FLOWBYTE_SALES];


GO
