CREATE TABLE [dbo].[sales_forecast_by_sales_channel_components]
(
	[company_id] NVARCHAR(10) NOT NULL, 
	[date] DATETIME NOT NULL,
    [sales_channel_code] NVARCHAR(255) NOT NULL, 
    [model] NVARCHAR(255) NOT NULL, 
    [trend] DECIMAL(38, 20) NULL, 
    [trend_lower] DECIMAL(38, 20) NULL, 
    [trend_upper] DECIMAL(38, 20) NULL, 
    [yearly_seasonality] DECIMAL(38, 20) NULL, 
    [yearly_seasonality_lower] DECIMAL(38, 20) NULL, 
    [yearly_seasonality_upper] DECIMAL(38, 20) NULL, 
    [weekly_seasonality] DECIMAL(38, 20) NULL, 
    [weekly_seasonality_lower] DECIMAL(38, 20) NULL, 
    [weekly_seasonality_upper] DECIMAL(38, 20) NULL, 
    [extra_regressors] DECIMAL(38, 20) NULL, 
    [extra_regressors_lower] DECIMAL(38, 20) NULL, 
    [extra_regressors_upper] DECIMAL(38, 20) NULL, 
    [specific_regressors] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_sales_forecast_by_sales_channel_components] PRIMARY KEY NONCLUSTERED ([company_id], [date], [sales_channel_code], [model]), 
    CONSTRAINT [FK_sales_forecast_by_sales_channel_components_sales_channel] FOREIGN KEY ([company_id], [sales_channel_code]) REFERENCES [sales_channel]([company_id], [code]) 
)
ON [FLOWBYTE_SALES];

GO

CREATE CLUSTERED INDEX [IX_sales_forecast_by_sales_channel_components_date] ON [dbo].[sales_forecast_by_sales_channel_components] ([date])
ON [FLOWBYTE_SALES];
