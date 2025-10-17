CREATE TABLE [Loan].[CustomerPhone]
(
	[CustomerPhoneID] BIGINT NOT NULL PRIMARY KEY,
	[CustomerID] BIGINT NOT NULL,
	[Type] SMALLINT NOT NULL,
	[Number] NVARCHAR(20) NOT NULL,
	[CreatedOn] DATETIMEOFFSET NOT NULL,
	[LastModifiedOn] DATETIMEOFFSET NULL,
	[SearchTerms] NVARCHAR(200) NULL,
	CONSTRAINT FK_CustomerPhone_CustomerID FOREIGN KEY ([CustomerID]) REFERENCES [Loan].[Customer] ([CustomerID]) ON DELETE CASCADE,
)
GO
CREATE INDEX IX_CustomerPhone_CustomerID_SearchTerms
ON [Loan].[CustomerPhone] ([CustomerID], [SearchTerms]);
