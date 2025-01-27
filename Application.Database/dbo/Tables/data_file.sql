CREATE TABLE [dbo].[data_file] (
    [id]          NVARCHAR (255) NOT NULL,
    [file_name]   NVARCHAR (MAX) NULL,
    [directory]   NVARCHAR (MAX) NULL,
    [description] NVARCHAR (MAX) NULL,
    [tags]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_data_file] PRIMARY KEY CLUSTERED ([id] ASC)
)
ON [FLOWBYTE_DIM];

