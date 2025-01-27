CREATE TABLE [dbo].[sales]
(
	 company_id NVARCHAR(10) NOT NULL,
     [channel_no] NVARCHAR(255) NOT NULL,
    pos_no NVARCHAR(255) NOT NULL,
    transaction_no NVARCHAR(255) NOT NULL,
    line_no NVARCHAR(255) NOT NULL,
    item_no NVARCHAR(255) NOT NULL,
    [variant_code] NVARCHAR(255) NULL,
    receipt_no NVARCHAR(255) NOT NULL,
    barcode_no NVARCHAR(255) NOT NULL,
    sales_person NVARCHAR(255) NOT NULL,
    net_price DECIMAL(38, 20) NOT NULL,
    [discount_amount] DECIMAL(38, 20) NOT NULL, 
    [vat_amount] DECIMAL(38, 20) NOT NULL, 
    [total_amount] DECIMAL(38, 20) NOT NULL, 
    quantity DECIMAL(18, 2) NOT NULL,
    [date] DATE NOT NULL,
    [time] TIME NULL, 
     
    CONSTRAINT PK_sales PRIMARY KEY ([company_id], [line_no], [channel_no], [pos_no], [transaction_no]), 
    CONSTRAINT [FK_sales_company] FOREIGN KEY ([company_id]) REFERENCES [company]([id]),
    CONSTRAINT [FK_sales_company_id_item_no] FOREIGN KEY ([company_id],[item_no]) REFERENCES [item]([company_id],[item_no]), 
       
)
ON [FLOWBYTE_TRANS];
