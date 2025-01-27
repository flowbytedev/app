CREATE TABLE [dbo].[user_claim] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [user_id]     NVARCHAR (450) NOT NULL,
    [claim_type]  NVARCHAR (MAX) NULL,
    [claim_value] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_user_claim] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_user_claim_application_user_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[application_user] ([id])
)
ON [FLOWBYTE_DIM];


GO
CREATE NONCLUSTERED INDEX [IX_user_claim_user_id]
    ON [dbo].[user_claim]([user_id] ASC)
    ON [FLOWBYTE_DIM];

