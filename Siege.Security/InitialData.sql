declare @appID int
declare @appName nvarchar(256)
declare @appDescription nvarchar(1024)
declare @adminUserName nvarchar(256)
declare @consumerName nvarchar(256)
declare @consumerDescription nvarchar(1024)
declare @consumerID int
declare @adminID int

select @appName = 'Security Sample Portal'
select @appDescription = 'Security Sample Portal'
select @adminUserName = 'Administrator'

select @consumerName = 'Sample Consumer'
select @consumerDescription = 'Sample Consumer System Account'

declare @permissionID int
declare @roleID int


--Set up system data

insert into Consumers (Name, Description, IsActive)
VALUES
(@consumerName, @consumerDescription, 1)

insert into Applications (Name, Description, IsActive)
VALUES
(@appName, @appDescription, 1)

select @appID = ApplicationId from applications where Name = @appName
select @consumerID = ConsumerID from consumers where Name = @consumerName

insert into ConsumersInApplications (ConsumerID, ApplicationID) VALUES (@consumerID, @appID)


--Set up initial permissions and roles

INSERT INTO [Permissions] (ApplicationID, Name, [Description], IsActive, ExcludeFromAssignment)
VALUES
(@appID, 'CanAdministerAllSecurity', 'User is able to administer security for ALL applications', 1, 1)

INSERT INTO Roles (ConsumerID, Name, [Description], IsActive)
VALUES
(@consumerID, 'Administrator', 'Administrator Role', 1)

select @permissionID = PermissionID from [Permissions] where Name = 'CanAdministerAllSecurity'
select @roleID = RoleID from Roles where Name = 'Administrator'

INSERT INTO PermissionsInRoles (RoleID, PermissionID) VALUES (@roleID, @permissionID)

--create administrator account, password is test1234

INSERT [Users] ([ConsumerID], [UserName], [Password], [PasswordSalt], [SecretQuestion], [SecretAnswer], [Email], [IsActive], [IsLockedOut], [FailedPasswordAttemptCount], [FailedSecretAnswerAttemptCount], 
[LastPasswordChangeDate], [CreateDate]) 
VALUES (@consumerID, @adminUserName, N'o6jOCXI7AZ8CNDeiQQtHPd+EIcyfzZv2CiMLKsEEfwU=', N'wvmlhU9KzMg=', N'What is the meaning of life?', N'43', N'test1234', 1, 0, 0, 0, CAST(0x0000A01F014BDFD6 AS DateTime), 
CAST(0x0000A01F014BDFD6 AS DateTime))

select @adminID = UserID from Users where UserName = @adminUserName

--assign initial roles to administrator

Insert into UsersInRoles (UserID, RoleID) VALUES (@adminID, @roleID)

--add admin to portal

Insert into UsersInApplications (UserID, ApplicationID) VALUES (@adminID, @appID)
