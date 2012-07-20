USE [Security]
GO
/****** Object:  Table [dbo].[Applications]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applications](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Consumers]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consumers](
	[ConsumerID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_security_Consumers] PRIMARY KEY CLUSTERED 
(
	[ConsumerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ConsumersInApplications]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConsumersInApplications](
	[ConsumerID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Groups]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[ConsumerID] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__security_G__149AF36B8ECB33F3] PRIMARY KEY NONCLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[PermissionID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[IsActive] [bit] NOT NULL,
	[ExcludeFromAssignment] [bit] NOT NULL,
 CONSTRAINT [PK_aspnet_Permissions] PRIMARY KEY CLUSTERED 
(
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PermissionsInRoles]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionsInRoles](
	[RoleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
 CONSTRAINT [PK_PermissionsInRoles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[ConsumerID] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__security_R__8AFACE1BA671ACC8] PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RolesInGroups]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolesInGroups](
	[GroupId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_RolesInGroups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[ConsumerID] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](256) NOT NULL,
	[PasswordSalt] [nvarchar](256) NOT NULL,
	[SecretQuestion] [nvarchar](256) NULL,
	[SecretAnswer] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[IsActive] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedSecretAnswerAttemptCount] [int] NOT NULL,
	[LastPasswordChangeDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_security_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsersInApplications]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersInApplications](
	[UserID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsersInGroups]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersInGroups](
	[UserID] [int] NOT NULL,
	[GroupID] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsersInRoles]    Script Date: 3/28/2012 6:41:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersInRoles](
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Applications] ADD  CONSTRAINT [DF_Applications_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_aspnet_Groups_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Permissions] ADD  CONSTRAINT [DF_aspnet_Permissions_ExcludeFromAssignment]  DEFAULT ((0)) FOR [ExcludeFromAssignment]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_aspnet_Roles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_security_Users_FailedPasswordAttemptCount]  DEFAULT ((0)) FOR [FailedPasswordAttemptCount]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_security_Users_FailedSecretAnswerAttemptCount]  DEFAULT ((0)) FOR [FailedSecretAnswerAttemptCount]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_security_Users_LastPasswordChangeDate]  DEFAULT (getdate()) FOR [LastPasswordChangeDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_security_Users_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ConsumersInApplications]  WITH CHECK ADD  CONSTRAINT [FK_security_ConsumersInApplications_security_Applications] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
GO
ALTER TABLE [dbo].[ConsumersInApplications] CHECK CONSTRAINT [FK_security_ConsumersInApplications_security_Applications]
GO
ALTER TABLE [dbo].[ConsumersInApplications]  WITH CHECK ADD  CONSTRAINT [FK_security_ConsumersInApplications_security_Consumers] FOREIGN KEY([ConsumerID])
REFERENCES [dbo].[Consumers] ([ConsumerID])
GO
ALTER TABLE [dbo].[ConsumersInApplications] CHECK CONSTRAINT [FK_security_ConsumersInApplications_security_Consumers]
GO
ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_security_Groups_security_Consumers] FOREIGN KEY([ConsumerID])
REFERENCES [dbo].[Consumers] ([ConsumerID])
GO
ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_security_Groups_security_Consumers]
GO
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_security_Permissions_security_Applications] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_security_Permissions_security_Applications]
GO
ALTER TABLE [dbo].[PermissionsInRoles]  WITH CHECK ADD  CONSTRAINT [FK_PermissionsInRoles_Permissions] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permissions] ([PermissionID])
GO
ALTER TABLE [dbo].[PermissionsInRoles] CHECK CONSTRAINT [FK_PermissionsInRoles_Permissions]
GO
ALTER TABLE [dbo].[PermissionsInRoles]  WITH CHECK ADD  CONSTRAINT [FK_PermissionsInRoles_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[PermissionsInRoles] CHECK CONSTRAINT [FK_PermissionsInRoles_Roles]
GO
ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [FK_security_Roles_security_Consumers] FOREIGN KEY([ConsumerID])
REFERENCES [dbo].[Consumers] ([ConsumerID])
GO
ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [FK_security_Roles_security_Consumers]
GO
ALTER TABLE [dbo].[RolesInGroups]  WITH CHECK ADD  CONSTRAINT [FK_RolesInGroups_Groups] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([GroupId])
GO
ALTER TABLE [dbo].[RolesInGroups] CHECK CONSTRAINT [FK_RolesInGroups_Groups]
GO
ALTER TABLE [dbo].[RolesInGroups]  WITH CHECK ADD  CONSTRAINT [FK_RolesInGroups_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[RolesInGroups] CHECK CONSTRAINT [FK_RolesInGroups_Roles]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_security_Users_security_Consumers] FOREIGN KEY([ConsumerID])
REFERENCES [dbo].[Consumers] ([ConsumerID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_security_Users_security_Consumers]
GO
ALTER TABLE [dbo].[UsersInApplications]  WITH CHECK ADD  CONSTRAINT [FK_security_UsersInApplications_security_Applications] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
GO
ALTER TABLE [dbo].[UsersInApplications] CHECK CONSTRAINT [FK_security_UsersInApplications_security_Applications]
GO
ALTER TABLE [dbo].[UsersInApplications]  WITH CHECK ADD  CONSTRAINT [FK_security_UsersInApplications_security_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersInApplications] CHECK CONSTRAINT [FK_security_UsersInApplications_security_Users]
GO
ALTER TABLE [dbo].[UsersInGroups]  WITH CHECK ADD  CONSTRAINT [FK_UsersInGroups1_Groups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupId])
GO
ALTER TABLE [dbo].[UsersInGroups] CHECK CONSTRAINT [FK_UsersInGroups1_Groups]
GO
ALTER TABLE [dbo].[UsersInGroups]  WITH CHECK ADD  CONSTRAINT [FK_UsersInGroups1_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersInGroups] CHECK CONSTRAINT [FK_UsersInGroups1_Users]
GO
ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersInRoles_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [FK_UsersInRoles_Roles]
GO
ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersInRoles_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [FK_UsersInRoles_Users]
GO
