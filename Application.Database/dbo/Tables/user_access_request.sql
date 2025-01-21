CREATE TABLE [dbo].[user_access_request] (
    [id]                  INT            IDENTITY (1, 1) NOT NULL,
    [company_id]          NVARCHAR (10)  NULL,
    [application_user_id] NVARCHAR (450) NULL,
    [application_page_id] NVARCHAR (255) NULL,
    [department]          NVARCHAR (MAX) NOT NULL,
    [manager_name]        NVARCHAR (MAX) NOT NULL,
    [reason]              NVARCHAR (MAX) NOT NULL,
    [created_on]          DATETIME2 (7)  NULL,
    [modified_on]         DATETIME2 (7)  NULL,
    [created_by]          NVARCHAR (MAX) NULL,
    [modified_by]         NVARCHAR (MAX) NULL,
    [is_deleted]          BIT            NULL,
    CONSTRAINT [PK_user_access_request] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_user_access_request_application_page_application_page_id] FOREIGN KEY ([application_page_id]) REFERENCES [dbo].[application_page] ([id]),
    CONSTRAINT [FK_user_access_request_application_user_application_user_id] FOREIGN KEY ([application_user_id]) REFERENCES [dbo].[application_user] ([id]),
    CONSTRAINT [FK_user_access_request_company_company_id] FOREIGN KEY ([company_id]) REFERENCES [dbo].[company] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_user_access_request_application_page_id]
    ON [dbo].[user_access_request]([application_page_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_user_access_request_application_user_id]
    ON [dbo].[user_access_request]([application_user_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_user_access_request_company_id]
    ON [dbo].[user_access_request]([company_id] ASC);

