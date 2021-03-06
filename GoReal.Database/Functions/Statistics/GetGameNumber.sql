﻿CREATE FUNCTION [dbo].[GetGameNumber]( 
	@UserId INT)
RETURNS INT

BEGIN
	RETURN (SELECT COUNT(GameId) FROM [Game] WHERE ([BlackPlayerId] = @UserId OR [WhitePlayerId] = @UserId) AND [WhitePlayerId] != [BlackPlayerId])
END