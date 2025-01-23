CREATE TABLE [dbo].[item]
(
	[company_id] NVARCHAR(10) NOT NULL,
    item_no [nvarchar](255) NOT NULL, 
    [title] NVARCHAR(255) NULL,
    [description] NVARCHAR(MAX) NULL, 
    [brand] NVARCHAR(255) NULL, 
    CONSTRAINT [PK_item] PRIMARY KEY ([company_id], [item_no]), 
    CONSTRAINT [FK_item_company] FOREIGN KEY ([company_id]) REFERENCES [company]([id])     
     
)


GO

CREATE INDEX [IX_item_title] ON [dbo].[item] ([title])

GO

CREATE INDEX [IX_item_brand] ON [dbo].[item] ([brand])
