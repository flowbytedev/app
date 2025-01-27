CREATE TABLE [dbo].[sales_forecast_by_group]
(
	 [date] DATETIME NOT NULL, 
    [store_group] NVARCHAR(10) NOT NULL, 
    [net_amount_acy] DECIMAL(38, 20) NOT NULL, 
    [net_amount_acy_upper] DECIMAL(38, 20) NOT NULL, 
    [net_amount_acy_lower] DECIMAL(38, 20) NOT NULL, 
    CONSTRAINT [PK_sales_forecast_by_group] PRIMARY KEY NONCLUSTERED ([date], [store_group]) 
)
ON [FLOWBYTE_SALES];

GO

CREATE CLUSTERED INDEX [IX_sales_forecast_by_group_Column] ON [dbo].[sales_forecast_by_group] ([date]) 
ON [FLOWBYTE_SALES];

