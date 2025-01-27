CREATE TABLE [dbo].[customer]
(
	[company_id] NVARCHAR(10) NOT NULL,
    [customer_id] INT NOT NULL , 
    [first_name] NVARCHAR(255) NOT NULL, 
    [last_name] NVARCHAR(255) NOT NULL, 
    [email] NVARCHAR(255) NULL,
    [contact_method] NVARCHAR(255) NOT NULL,
    [contact_number] NVARCHAR(255) NOT NULL, 
    [date_of_birth] DATE NOT NULL, 
    [date_joined] DATE NOT NULL, 
    [status] NVARCHAR(255) NULL, 
    [gender] NVARCHAR(255) NOT NULL, 
    [note] NVARCHAR(MAX) NULL, 
    [customer_type] NVARCHAR(255) NULL, 
    CONSTRAINT [PK_customer] PRIMARY KEY ([company_id], [customer_id]), 
    CONSTRAINT [FK_customer_company] FOREIGN KEY ([company_id]) REFERENCES [company]([id]), 
    CONSTRAINT [FK_customer_status] FOREIGN KEY ([status]) REFERENCES [customer_status]([code]), 
    CONSTRAINT [FK_customer_customer_type] FOREIGN KEY ([customer_type]) REFERENCES [customer_type]([code]), 
     
    
)
GO


CREATE INDEX [IX_customer_company_id_customer_id] ON [dbo].[customer] ([company_id],[customer_id])
