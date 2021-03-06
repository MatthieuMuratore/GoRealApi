﻿CREATE FUNCTION [dbo].[GetUserRole]( 
	@UserId INT)
RETURNS INT
AS
BEGIN
    DECLARE @UserRole INT = 0;

    DECLARE @Roles TABLE ([RoleName] NVARCHAR(120));
	INSERT INTO @Roles SELECT R.[RoleName] FROM [Role] AS R JOIN [UserRole] AS UR
		ON R.[RoleId] = UR.[RoleId]
			WHERE UR.[UserId] = @UserId
	IF EXISTS (SELECT * FROM @Roles WHERE [RoleName] = 'Viewer')
		SET @UserRole += 1;
	IF EXISTS (SELECT * FROM @Roles WHERE [RoleName] = 'Player')
		SET @UserRole += 2;
	IF EXISTS (SELECT * FROM @Roles WHERE [RoleName] = 'Moderator')
		SET @UserRole += 4;
	IF EXISTS (SELECT * FROM @Roles WHERE [RoleName] = 'Administrator')
		SET @UserRole += 8;
	IF EXISTS (SELECT * FROM @Roles WHERE [RoleName] = 'SuperAdministrator')
		SET @UserRole += 16;
	RETURN @UserRole
END