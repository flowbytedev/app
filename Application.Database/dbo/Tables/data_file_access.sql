CREATE TABLE [dbo].[data_file_access] (
    [data_file_id]        NVARCHAR (450) NOT NULL,
    [application_user_id] NVARCHAR (450) NOT NULL,
    [access_type]         INT            NOT NULL,
    CONSTRAINT [PK_data_file_access] PRIMARY KEY CLUSTERED ([data_file_id] ASC, [application_user_id] ASC),
    CONSTRAINT [FK_data_file_access_application_user_application_user_id] FOREIGN KEY ([application_user_id]) REFERENCES [dbo].[application_user] ([id]),
    CONSTRAINT [FK_data_file_access_data_file_data_file_id] FOREIGN KEY ([data_file_id]) REFERENCES [dbo].[data_file] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_data_file_access_application_user_id]
    ON [dbo].[data_file_access]([application_user_id] ASC);

