CREATE TABLE [dbo].[variant_option] (
    optionid INT IDENTITY(1,1) PRIMARY KEY,
    itemnumber INT NOT NULL,
    optname NVARCHAR(50) NOT NULL,
)