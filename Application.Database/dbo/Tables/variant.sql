CREATE TABLE [dbo].[variant]
(
	[cdc_key] [bigint] NULL,
	[company_id] [nvarchar](10) NOT NULL,
	[item_no] [nvarchar](255) NOT NULL,
	[variant_code] [nvarchar](255) NOT NULL,
	[description] [nvarchar](255) NULL,
	[status] [int] NOT NULL,
	[option_1] [nvarchar](255) NULL,
	[value_1] [nvarchar](255) NULL,
	[option_2] [nvarchar](255) NULL,
	[value_2] [nvarchar](255) NULL,
	[option_3] [nvarchar](255) NULL,
	[value_3] [nvarchar](255) NULL,
	[packaging_material] [int] NULL,
	[width] [decimal](38, 20) NULL,
	[height] [decimal](38, 20) NULL,
	[depth] [decimal](38, 20) NULL, 
    CONSTRAINT [PK_variant] PRIMARY KEY ([company_id], [item_no], [variant_code]) 
)
