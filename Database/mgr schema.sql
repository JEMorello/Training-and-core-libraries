
/* 
  Important Notes:
	Though most of the conent is planned for an "mgr" schema, the type elements written here are useful enough to go to
	the "dbo" or other generic use schema point. I have chosen "dbo".
Create Schema mgr Authorization dbo

*/
CREATE TABLE mgr.AUser
(
	UserID INT IDENTITY(1,1) PRIMARY KEY,
	Username VARCHAR(25) NOT NULL,
	Password VARCHAR(25) NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE(),
	CONSTRAINT UQ_Username UNIQUE(Username)
)

CREATE TABLE mgr.Claim
(
	ClaimID INT IDENTITY(1,1) PRIMARY KEY,
	ClaimName VARCHAR(25) NOT NULL,
	ClaimDescription VARCHAR(250),
	CreatedDate DATETIME DEFAULT GETDATE(),
	CONSTRAINT UQ_ClaimNName UNIQUE(ClaimName)
)

CREATE TABLE mgr.ARole
(
	RoleID INT IDENTITY(1,1) PRIMARY KEY,
	RoleName VARCHAR(25) NOT NULL,
	RoleDescription VARCHAR(250),
	CreatedDate DATETIME DEFAULT GETDATE(),
	CONSTRAINT UQ_RoleName UNIQUE(RoleName)
)

CREATE TABLE mgr.AccessPoint
(
	APID INT IDENTITY(1,1) PRIMARY KEY,
	AccessPointName VARCHAR(50) NOT NULL,
	AccessPointDescription VARCHAR(250),
	APURL VARCHAR(250),
	CreatedDate DATETIME DEFAULT GETDATE(),
	CONSTRAINT UQ_AccessPointName UNIQUE(AccessPointName)
)

CREATE TABLE mgr.UserClaims
(
	UserClaimID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT NOT NULL,
	ClaimID INT NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_UserID FOREIGN KEY (UserID)
	REFERENCES mgr.AUser(UserID),
	CONSTRAINT FK_ClaimID FOREIGN KEY (ClaimID)
	REFERENCES mgr.Claim(ClaimID)
)

CREATE TABLE mgr.ClaimsAccessPoints
(
	ClaimAPID INT IDENTITY(1,1) PRIMARY KEY,
	ClaimID INT NOT NULL,
	APID INT NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_ClaimIDAP FOREIGN KEY (ClaimID)
	REFERENCES mgr.Claim(ClaimID),
	CONSTRAINT FK_APID FOREIGN KEY (APID)
	REFERENCES mgr.AccessPoint(APID)
)

CREATE TABLE mgr.RoleClaims
(
	RoleClaimID INT IDENTITY(1,1) PRIMARY KEY,
	RoleID INT NOT NULL,
	ClaimID INT NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE(),
	CONSTRAINT FK_RoleID FOREIGN KEY (RoleID)
	REFERENCES mgr.ARole(RoleID),
	CONSTRAINT FK_ClaimID_RC FOREIGN KEY (ClaimID)
	REFERENCES mgr.Claim(ClaimID)
)

GO

/* Types */
CREATE TYPE mgr.ElementList
AS TABLE
(
	ElementID INT
)

CREATE TYPE mgr.DictionaryGroup
AS TABLE
(
	Part1ID INT,
	Part1Name VARCHAR(50),
	Part2ID INT,
	Part2Name VARCHAR(50)
)
GO

/* Functions */
CREATE FUNCTION mgr.FetchUserID
(
	@UserName AS VARCHAR(25)
)
RETURNS INT
AS
BEGIN
	DECLARE @UserID AS INT
	SELECT TOP 1 @UserID = UserID
	FROM mgr.AUser
	WHERE UserName = @UserName
	RETURN @UserID
END
GO	

/* Procedures */
CREATE PROCEDURE mgr.AddUser
(
	@UserName VARCHAR(25),
	@Password VARCHAR(25)
)
AS
BEGIN
	/* has a direct output of the ID created, Use EXECUTE SCALAR From Code. */
	INSERT INTO mgr.AUSER
		(UserName, Password)
		OUTPUT INSERTED.UserID
	VALUES (@UserName, @Password)
END
GO

CREATE PROCEDURE mgr.DeleteUser
(
	@UserID INT
)
AS
BEGIN
	DELETE FROM mgr.AUSER
	WHERE UserID = @UserID
END
GO

CREATE PROCEDURE mgr.AddClaim
(
	@ClaimName VARCHAR(50),
	@ClaimDescription VARCHAR(250) = NULL
)
AS
BEGIN
	/* has a direct output of the ID created, Use EXECUTE SCALAR From Code. */
	INSERT INTO mgr.Claim
		(ClaimName, ClaimDescription)
		OUTPUT INSERTED.ClaimID
	VALUES(@ClaimName, @ClaimDescription)

END
GO

CREATE PROCEDURE mgr.DeleteClaim
(
	@ClaimID INT
)
AS
BEGIN
	DELETE FROM mgr.Claim
	WHERE ClaimID = @ClaimID
END
GO

CREATE PROCEDURE mgr.AddRole
(
	@RoleName VARCHAR(25),
	@RoleDescription VARCHAR(250) = NULL
)
AS
BEGIN
	/* has a direct output of the ID created, Use EXECUTE SCALAR From Code. */
	INSERT INTO mgr.ARole
		(RoleName, RoleDescription)
		OUTPUT INSERTED.RoleID
	VALUES(@RoleName, @RoleDescription)
END
GO

CREATE PROCEDURE mgr.DeleteRole
(
	@RoleID INT
)
AS
BEGIN
	DELETE FROM mgr.ARole
	WHERE RoleID = @RoleID
END
GO

CREATE PROCEDURE mgr.AddAccessPoint
(
	@AccessPointName VARCHAR(50),
	@APDescription VARCHAR(250) = NULL,
	@APURL VARCHAR(250) = NULL
)
AS
BEGIN
	/* has a direct output of the ID created, Use EXECUTE SCALAR From Code. */
	INSERT INTO mgr.AccessPoint
		(AccessPointName, AccessPointDescription, APURL)
		OUTPUT INSERTED.APID
	VALUES(@AccessPointName, @APDescription, @APURL)
END

GO

CREATE PROCEDURE mgr.DeleteAccessPoint
(
	@APID INT
)
AS
BEGIN
	DELETE FROM mgr.AccessPoint
	WHERE APID = @APID
END
GO

CREATE PROCEDURE mgr.GrantClaimsToUser
(
	@UserName VARCHAR(25),
	@CList as mgr.ElementList REadonly
)
AS
BEGIN
	DECLARE @UserID as INT
	SET @UserID = mgr.FetchUserID(@UserName)
	
	INSERT INTO UserClaims
		(UserID, ClaimID)
	SELECT  
		@UserID
		, C.ClaimID
	FROM Claim C
	INNER JOIN @CList L
	ON C.ClaimID = L.ElementID
	LEFT OUTER JOIN UserClaims UC
	ON C.ClaimID = UC.ClaimID
	AND UC.UserID = @UserID
	WHERE UC.UserID IS NULL
END
GO

CREATE PROCEDURE mgr.RemoveClaimsFromUser
(
	@UserName VARCHAR(25),
	@CList as mgr.ElementList REadonly
)
AS
BEGIN
	DELETE UC FROM UserClaims UC
	INNER JOIN @CList L
	ON UC.ClaimID = L.ElementID
	AND UC.ClaimID = mgr.FetchUserID(@UserName)
	
END
GO

CREATE PROCEDURE mgr.GrantAccessPointsToClaim
(
	@ClaimID AS INT,
	@APList AS ElementList REadonly
)
AS
BEGIN
	INSERT INTO ClaimsAccessPoints
		(ClaimID, APID)
	SELECT @ClaimID, AP.APID
	FROM AccessPoint AP
	INNER JOIN @APList L
	ON AP.APID = L.ElementID
	INNER JOIN Claim C
	ON C.ClaimID = @ClaimID
	LEFT OUTER JOIN ClaimsAccessPoints CAP
	ON CAP.ClaimID = C.ClaimID
	AND CAP.APID = AP.APID
	WHERE CAP.ClaimID IS NULL
	
END
GO

CREATE PROCEDURE mgr.RemoveAccessPointsFromClaim
(
	@ClaimID AS INT,
	@APList AS ElementList REadonly
)
AS
BEGIN
	DELETE CAP FROM ClaimsAccessPoints CAP
	INNER JOIN @APList L
	ON CAP.APID = L.ElementID
	WHERE CAP.ClaimID = @ClaimID
END
GO

CREATE PROCEDURE mgr.GrantClaimsToAccessPoint
(
	@APID AS INT,
	@CList AS ElementList REadonly
)
AS
BEGIN
	INSERT INTO ClaimsAccessPoints
		(ClaimID, APID)
	SELECT C.ClaimID, @APID
	FROM Claim C
	INNER JOIN @CList L
	ON C.ClaimID = L.ElementID
	INNER JOIN AccessPoint AP
	ON AP.APID = @APID
	LEFT OUTER JOIN ClaimsAccessPoints CAP
	ON CAP.ClaimID = CAP.ClaimID
	AND CAP.APID = AP.APID
	WHERE CAP.APID IS NULL
	
END
GO

CREATE PROCEDURE mgr.RemoveClaimsFromAccessPoint
(
	@APID AS INT,
	@CList AS ElementList ReadOnly
)
AS
BEGIN
	DELETE CAP FROM ClaimsAccessPoints CAP
	INNER JOIN @CList L
	ON  CAP.ClaimID = L.ElementID
	WHERE CAP.APID = @APID
END
GO

CREATE PROCEDURE mgr.GrantClaimsToRole
(
	@RoleID AS INT,
	@CList AS ElementList ReadOnly
)
AS
BEGIN
	INSERT INTO RoleClaims
	(RoleID, ClaimID)
	SELECT 
		@RoleID
		, C.ClaimID
	FROM Claim C
	INNER JOIN @CList L
	ON C.ClaimID = L.ElementID
	LEFT OUTER JOIN RoleClaims RC
	ON RC.RoleID = @RoleID
	AND RC.ClaimID = C.ClaimID
	WHERE RC.ClaimID IS NULL
	
END
GO

CREATE PROCEDURE mgr.RemoveClaimsFromRole
(
	@RoleID AS INT,
	@CList AS ElementList ReadOnly
)
AS
BEGIN
	DELETE RC FROM RoleClaims RC
	INNER JOIN @CList L
	ON RC.ClaimID = L.ElementID
	WHERE RC.RoleID = @RoleID
	
END
GO

CREATE PROCEDURE mgr.ApplyRoleToUser
(
	@RoleID AS INT,
	@UserName AS VARCHAR(25)
)
AS
BEGIN
	DECLARE @UserID AS INT
	SELECT @UserID = mgr.FetchUserID(@UserName)
	
	INSERT INTO UserClaims
	(UserID, ClaimID)
	SELECT @UserID, C.ClaimID
	FROM Claim C
	INNER JOIN RoleClaims RC
	ON C.ClaimID = RC.ClaimID
	AND RC.RoleID = @RoleID
	LEFT OUTER JOIN UserClaims UC
	ON UC.UserID = @UserID
	AND UC.ClaimID = C.ClaimID
	WHERE UC.UserID IS NULL
	
END
GO

CREATE PROCEDURE mgr.RemoveRoleFromUser
(
	@RoleID AS INT,
	@UserName AS VARCHAR(25)
)
AS
BEGIN
	DECLARE @UserID AS INT
	SELECT @UserID = mgr.FetchUserID(@UserName)
	
	DELETE UC FROM UserClaims UC
	INNER JOIN RoleClaims RC
	ON RC.ClaimID = UC.ClaimID
	AND RC.RoleID = @RoleID
	INNER JOIN Claim C
	ON UC.ClaimID = C.ClaimID
	WHERE UC.UserID = @UserID

END
GO

CREATE PROCEDURE mgr.CloneUser
(
	@UserSource AS VARCHAR(25),
	@UserDestination AS VARCHAR(25)
)
AS
BEGIN
	DECLARE @UserID1 AS INT
	DECLARE @UserID2 AS INT
	SELECT @UserID1 = mgr.FetchUserID(@UserSource)
	SELECT @UserID2 = mgr.FetchUserID(@UserDestination)
	
	INSERT INTO UserClaims
	(UserID, ClaimID)
	SELECT @UserID2, C.ClaimID
	FROM Claim C
	INNER JOIN UserClaims SC
	ON SC.UserID = @UserID1
	AND C.ClaimID = SC.ClaimID
	LEFT OUTER JOIN UserClaims DC
	ON DC.UserID = @UserID1
	AND DC.ClaimID = C.ClaimID
	WHERE DC.ClaimID IS NULL
	
END
GO

		