CREATE TABLE [Loan].[Customer]
(
	[CustomerID] BIGINT NOT NULL PRIMARY KEY,
	[FirstName] NVARCHAR(50) NOT NULL,
	[MiddleName] NVARCHAR(50) NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[DocumentNumber] NVARCHAR(20) NOT NULL,
	[DateOfBirth] DATE NULL,
	[IsActive] BIT NOT NULL DEFAULT(1),
	[CreatedOn] DATETIMEOFFSET NOT NULL,
	[LastModifiedOn] DATETIMEOFFSET NULL,
	[SearchTerms] NVARCHAR(200) NULL,
)
GO
CREATE INDEX IX_Customer_Year_Name_SearchTerms
ON [Loan].[Customer] ([FirstName], [SearchTerms])
INCLUDE ([LastName], [DocumentNumber], [DateOfBirth]);
