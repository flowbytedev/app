CREATE TABLE [dbo].[application_page_role] (
    [application_page_id] NVARCHAR (255) NOT NULL,
    [identity_role_id]    NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_application_page_role] PRIMARY KEY CLUSTERED ([application_page_id] ASC, [identity_role_id] ASC),
    CONSTRAINT [FK_application_page_role_application_page_application_page_id] FOREIGN KEY ([application_page_id]) REFERENCES [dbo].[application_page] ([id]),
    CONSTRAINT [FK_application_page_role_role_identity_role_id] FOREIGN KEY ([identity_role_id]) REFERENCES [dbo].[role] ([id])

)
ON [FLOWBYTE_DIM];


GO
CREATE NONCLUSTERED INDEX [IX_application_page_role_identity_role_id]
    ON [dbo].[application_page_role]([identity_role_id] ASC)
    ON [FLOWBYTE_DIM];

