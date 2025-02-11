CREATE TABLE [dbo].[field_mapping](
	[source_host] NVARCHAR(255) NOT NULL, 
    [source_database] NVARCHAR(255) NOT NULL,
	[source_table] [nvarchar](255) NOT NULL,
	[source_column] [nvarchar](255) NOT NULL,
	[destination_host] NVARCHAR(255) NOT NULL, 
    [destination_database] NVARCHAR(255) NOT NULL,
	[destination_table] [nvarchar](255) NOT NULL,
	[destination_column] [nvarchar](255) NOT NULL,
	[source_data_type] [nvarchar](255) NOT NULL,
	[destination_data_type] [nvarchar](255) NOT NULL,
	[is_group_by] [bit] NULL,
	[is_sum] [bit] NULL,
	[is_count] [bit] NULL,
	[filter_query] [nvarchar](max) NULL,
	[default_value] [nvarchar](max) NULL, 
    [is_attribute] TINYINT NULL, 
    [is_attribute_key] TINYINT NULL, 
    [temp_table_name] NVARCHAR(255) NULL
	 
     
) ON [FLOWBYTE_DIM] TEXTIMAGE_ON [FLOWBYTE_DIM]
GO