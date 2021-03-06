﻿CREATE PROCEDURE [dbo].[GameCreate]
	@Size INT,
	@Komi INT,
	@Handicap INT,
	@TimeControlId INT = NULL,
	@RuleId INT,
	@BlackPlayerId INT,
	@WhitePlayerId INT
AS
BEGIN
	INSERT INTO [Game] ([StartDate], [BlackRank], [WhiteRank], [Size], [Komi], [Handicap], [TimeControlId], [RuleId], [BlackPlayerId], [WhitePlayerId])
		VALUES (GETDATE(), dbo.GetPlayerRanking(@BlackPlayerId), dbo.GetPlayerRanking(@WhitePlayerId), @Size, @Komi, @Handicap, @TimeControlId, @RuleId, @BlackPlayerId, @WhitePlayerId)
	SELECT * FROM [Game] WHERE [GameId] = CAST(SCOPE_IDENTITY() AS INT)
END