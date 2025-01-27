CREATE TABLE [dbo].[company_member] (
    [company_id]          NVARCHAR (10)  NOT NULL,
    [application_user_id] NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_company_member] PRIMARY KEY NONCLUSTERED ([company_id] ASC, [application_user_id] ASC),
    CONSTRAINT [FK_company_member_application_user_application_user_id] FOREIGN KEY ([application_user_id]) REFERENCES [dbo].[application_user] ([id]),
    CONSTRAINT [FK_company_member_company_company_id] FOREIGN KEY ([company_id]) REFERENCES [dbo].[company] ([id])
)
ON [FLOWBYTE_DIM];


GO
CREATE NONCLUSTERED INDEX [IX_company_member_application_user_id]
    ON [dbo].[company_member]([application_user_id] ASC)
    ON [FLOWBYTE_DIM];

