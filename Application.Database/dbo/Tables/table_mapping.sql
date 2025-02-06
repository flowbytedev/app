CREATE TABLE [dbo].[table_mapping]
(
	[source_host] NVARCHAR(255) NULL, 
    [source_database] NVARCHAR(255) NULL,
	[source_table] [nvarchar](255) NULL,
	[source_api_endpoint] NVARCHAR(MAX) NULL, 
	[destincation_host] NVARCHAR(255) NULL, 
    [destination_database] NVARCHAR(255) NULL,
	[destination_table] [nvarchar](255) NULL,
	[destination_api_endpoint] NVARCHAR(MAX) NULL,
	[query] [nvarchar](MAX) NULL, 
    [is_attribute] TINYINT NULL,
) ON [FLOWBYTE_DIM] TEXTIMAGE_ON [FLOWBYTE_DIM]
