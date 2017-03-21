USE Core

CREATE TABLE ftr.Account
(
	AccountID  INT IDENTITY(1,1) PRIMARY KEY,
	AccountTypeID INT NOT NULL,
	ContactID INT 
	
)

CREATE TABLE ftr.AccountType
(
	AccountTypeID INT IDENTITY(1,1) PRIMARY KEY,
	AccountTypeDescription VARCHAR(50),
	Oreintation INT
	
)
	
CREATE TABLE ftr.FTransaction
(
	FTID  INT IDENTITY(1,1) PRIMARY KEY,
	FValue MONEY DEFAULT (0),
	FTSouce INT NOT NULL,
	FTDestination INT NOT NULL,
	AnnotationID INT
)

