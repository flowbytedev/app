CREATE TABLE [dbo].[role] (
    [id]                NVARCHAR (450) NOT NULL,
    [name]              NVARCHAR (256) NULL,
    [normalized_name]   NVARCHAR (256) NULL,
    [concurrency_stamp] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_role] PRIMARY KEY CLUSTERED ([id] ASC)
)
ON [FLOWBYTE_DIM];


GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
    ON [dbo].[role]([normalized_name] ASC) WHERE ([normalized_name] IS NOT NULL)
    ON [FLOWBYTE_DIM];

