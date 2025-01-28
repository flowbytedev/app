CREATE TABLE [dbo].[sales_channel] (
    [company_id]  NVARCHAR (10)  NOT NULL,
    [code]        NVARCHAR (255) NOT NULL,
    [name]        NVARCHAR (MAX) NULL,
    [region]      NVARCHAR (MAX) NULL,
    [address]     NVARCHAR (MAX) NULL,
    [store_group] NVARCHAR (MAX) NULL,
    [created_on]  DATETIME2 (7)  NULL,
    [modified_on] DATETIME2 (7)  NULL,
    [created_by]  NVARCHAR (MAX) NULL,
    [modified_by] NVARCHAR (MAX) NULL,
    [is_deleted]  BIT            NULL,
    CONSTRAINT [PK_sales_channel] PRIMARY KEY CLUSTERED ([company_id] ASC, [code] ASC),
    CONSTRAINT [FK_sales_channel_company_company_id] FOREIGN KEY ([company_id]) REFERENCES [dbo].[company] ([id])
)
ON [FLOWBYTE_DIM];

