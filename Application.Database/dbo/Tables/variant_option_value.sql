CREATE TABLE [dbo].[variant_option_value]
(
	valueid INT IDENTITY(1,1) PRIMARY KEY,
    optionid INT NOT NULL,
    optionvalue NVARCHAR(50) NOT NULL, 
)