CREATE TABLE [dbo].[item_attribute]
(
	[company_id] NVARCHAR(10) NOT NULL, 
    [item_no] NVARCHAR(255) NOT NULL, 
    [attribute] NVARCHAR(255) NOT NULL, 
    [value] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [PK_item_attribute] PRIMARY KEY ([company_id], [item_no], [attribute]), 
    CONSTRAINT [FK_item_attribute_company] FOREIGN KEY ([company_id]) REFERENCES [company]([id]), 
    CONSTRAINT [FK_item_attribute_item] FOREIGN KEY ([company_id],[item_no]) REFERENCES [item]([company_id], [item_no]) 
)
ON [FLOWBYTE_DIM];

GO

CREATE INDEX [IX_item_attribute_attribute] ON [dbo].[item_attribute] ([attribute])
ON [FLOWBYTE_DIM];
