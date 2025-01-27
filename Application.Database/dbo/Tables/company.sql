CREATE TABLE [dbo].[company] (
    [id]          NVARCHAR (10)  NOT NULL,
    [name]        NVARCHAR (MAX) NULL,
    [created_on]  DATETIME2 (7)  NULL,
    [modified_on] DATETIME2 (7)  NULL,
    [created_by]  NVARCHAR (MAX) NULL,
    [modified_by] NVARCHAR (MAX) NULL,
    [is_deleted]  BIT            NULL,
    CONSTRAINT [PK_company] PRIMARY KEY CLUSTERED ([id] ASC)
)
ON [FLOWBYTE_DIM];

