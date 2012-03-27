declare @appID uniqueidentifier
declare @appName nvarchar(256)
declare @loweredAppName nvarchar(256)
declare @appDescription nvarchar(1024)
declare @adminUserName nvarchar(256)

select @appName = 'DEFI'
select @loweredAppName = 'defi'
select @appDescription = 'DEFI Portal'
select @adminUserName = 'Administrator'

insert into [aspnet_SchemaVersions] ([Feature], CompatibleSchemaVersion, IsCurrentVersion) VALUES ('common', 1, 1)
insert into [aspnet_SchemaVersions] (Feature, CompatibleSchemaVersion, IsCurrentVersion) VALUES ('health monitoring', 1, 1)
insert into [aspnet_SchemaVersions] (Feature, CompatibleSchemaVersion, IsCurrentVersion) VALUES ('membership', 1, 1)
insert into [aspnet_SchemaVersions] (Feature, CompatibleSchemaVersion, IsCurrentVersion) VALUES ('personalization', 1, 1)
insert into [aspnet_SchemaVersions] (Feature, CompatibleSchemaVersion, IsCurrentVersion) VALUES ('profile', 1, 1)
insert into [aspnet_SchemaVersions] (Feature, CompatibleSchemaVersion, IsCurrentVersion) VALUES ('role manager', 1, 1)

insert into aspnet_Applications (ApplicationName, LoweredApplicationName, ApplicationID, Description, IsActive)
VALUES
(@appName, @loweredAppName, newid(), @appDescription, 1)

select @appID = ApplicationId from aspnet_applications where ApplicationName = @appName

INSERT INTO aspnet_Permissions (PermissionName, [Description], IsActive, ExcludeFromAssignment)
VALUES
('CanAdministerAllSecurity', 'User is able to administer security for ALL applications', 1, 1)

INSERT INTO aspnet_Roles (ApplicationId, RoleName, [Description], IsActive)
VALUES
(@appID, 'Administrator', 'Administrator Role', 1)

INSERT INTO aspnet_PermissionsInRoles (RoleID, PermissionID) VALUES (1,1)