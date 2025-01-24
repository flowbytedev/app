CREATE TABLE [dbo].[sales_forecast_by_scheme]
(
	 [date] DATETIME NOT NULL, 
    [store_scheme] NVARCHAR(10) NOT NULL, 
    [net_amount_acy] DECIMAL(38, 20) NOT NULL, 
    [net_amount_acy_upper] DECIMAL(38, 20) NOT NULL, 
    [net_amount_acy_lower] DECIMAL(38, 20) NOT NULL, 
    CONSTRAINT [PK_sales_forecast_by_scheme] PRIMARY KEY NONCLUSTERED ([date], [store_scheme]) 
)

GO

CREATE CLUSTERED INDEX [IX_sales_forecast_by_scheme_Column] ON [dbo].[sales_forecast_by_scheme] ([date]) 

