CREATE TABLE [dbo].[database] (
    [host]        NVARCHAR (255) NOT NULL,
    [name]        NVARCHAR (255) NOT NULL,
    [description] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_database] PRIMARY KEY CLUSTERED ([host] ASC, [name] ASC)
)
ON [FLOWBYTE_DIM];

