declare @userName nvarchar(256)
declare @userID uniqueidentifier
declare @roleID int

select @userID = UserID from aspnet_Users where UserName = 'Administrator'
select @RoleID = RoleID from aspnet_Roles where RoleName = 'Administrator'

insert into aspnet_UsersInRoles (UserID, RoleID) VALUES (@userId, @roleID)
