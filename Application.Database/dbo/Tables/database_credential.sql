CREATE TABLE [dbo].[database_credential] (
    [host]          NVARCHAR (450) NOT NULL,
    [database_name] NVARCHAR (450) NOT NULL,
    [username]      NVARCHAR (450) NOT NULL,
    [password]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_database_credential] PRIMARY KEY CLUSTERED ([host] ASC, [database_name] ASC, [username] ASC),
    CONSTRAINT [FK_database_credential_database_host_database_name] FOREIGN KEY ([host], [database_name]) REFERENCES [dbo].[database] ([host], [name])
)
ON [FLOWBYTE_DIM];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_database_credential_host_database_name]
    ON [dbo].[database_credential]([host] ASC, [database_name] ASC)
    ON [FLOWBYTE_DIM];

