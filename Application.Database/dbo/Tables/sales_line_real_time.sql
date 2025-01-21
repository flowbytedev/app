CREATE TABLE [dbo].[sales_line_real_time] (
    [date_time]          DATETIME2 (7)    NOT NULL,
    [store_code]         NVARCHAR (450)   NOT NULL,
    [net_amount_acy]     DECIMAL (38, 20) NOT NULL,
    [company_id]         NVARCHAR (450)   DEFAULT (N'') NOT NULL,
    [scheme]             NVARCHAR (MAX)   NULL,
    [total_transactions] INT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_sales_line_real_time] PRIMARY KEY CLUSTERED ([date_time] ASC, [company_id] ASC, [store_code] ASC)
);

