USE NewGalaxyDb
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<wangshibang>
-- Create date: <2018-03-14>
-- Description:	<>
-- =============================================
CREATE PROCEDURE dbo.SP_GetUserPermissions
	-- Add the parameters for the stored procedure here
	@UserId INT
AS
BEGIN
	DECLARE @PermissionIds NVARCHAR(MAX)
	DECLARE @ParentIds NVARCHAR(MAX)
	SELECT @PermissionIds = STUFF(
		(
			SELECT PermissionIds + ',' FROM dbo.RolePermissions p 
			LEFT JOIN dbo.UserRoles r ON p.RoleId=r.RoleId 
			INNER JOIN dbo.Users u ON r.UserId=u.Id 
			WHERE u.Id=@UserId FOR XML PATH('')),
		1,0,'')

	--获取ParentId集合
	SELECT @ParentIds = STUFF(
		(
			SELECT CAST(ParentNodeId AS VARCHAR) + ',' FROM dbo.Menus 
			WHERE CHARINDEX(rtrim(Id) +',', @PermissionIds)> 0 FOR XML PATH('')),
		1,0,'')

	SELECT Id, Code, CreateDate, Name, ParentNodeId, ParentNodeName, Remark, Status, URL, LevelCode, MenuIcon, MenuType FROM dbo.Menus
	WHERE CHARINDEX(RTRIM(Id)+',', @PermissionIds) > 0
	UNION
	SELECT Id, Code, CreateDate, Name, ParentNodeId, ParentNodeName, Remark, Status, URL, LevelCode, MenuIcon, MenuType FROM dbo.Menus
	WHERE CHARINDEX(RTRIM(Id)+',', @ParentIds) > 0
END
GO
