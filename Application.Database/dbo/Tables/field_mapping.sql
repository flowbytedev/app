CREATE TABLE [dbo].[field_mapping] (
    [source_table]          NVARCHAR (450) NOT NULL,
    [source_column]         NVARCHAR (450) NOT NULL,
    [destination_table]     NVARCHAR (450) NOT NULL,
    [destination_column]    NVARCHAR (450) NOT NULL,
    [source_data_type]      NVARCHAR (MAX) NOT NULL,
    [destination_data_type] NVARCHAR (MAX) NOT NULL,
    [is_group_by]           BIT            NULL,
    [is_sum]                BIT            NULL,
    [is_count]              BIT            NULL,
    [filter_query]          NVARCHAR (MAX) NULL,
    [default_value]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_field_mapping] PRIMARY KEY CLUSTERED ([source_table] ASC, [source_column] ASC, [destination_table] ASC, [destination_column] ASC)
)
ON [FLOWBYTE_DIM];

