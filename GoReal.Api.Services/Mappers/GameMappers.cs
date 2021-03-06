﻿using GoReal.Api.Models;
using D = GoReal.Dal.Entities;

namespace GoReal.Api.Services.Mappers
{
    public static class GameMappers
    {
        public static Game ToClient(this D.Game entity)
        {
            return new Game()
            {
                Id = entity.Id,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                BlackRank = entity.BlackRank,
                WhiteRank = entity.WhiteRank,
                Result = entity.Result,
                Size = entity.Size,
                Komi = entity.Komi,
                Handicap = entity.Handicap,
                BlackCapture = entity.BlackCapture,
                WhiteCapture = entity.WhiteCapture,
                BlackState = entity.BlackState,
                WhiteState = entity.WhiteState,
                KoInfo = entity.KoInfo.ToStone(),
                TimeControl = new D.TimeControl() { Id = entity.TimeControlId },
                Rule = new D.Rule() { Id = entity.RuleId },
                BlackPlayer = new User() { UserId = entity.BlackPlayerId },
                WhitePlayer = new User() { UserId = entity.WhitePlayerId },
            };
        }
        public static D.Game ToDal(this Game entity)
        {
            return new D.Game()
            {
                Id = entity.Id,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                BlackRank = entity.BlackRank,
                WhiteRank = entity.WhiteRank,
                Result = entity.Result,
                Size = entity.Size,
                Komi = entity.Komi,
                Handicap = entity.Handicap,
                BlackCapture = entity.BlackCapture,
                WhiteCapture = entity.WhiteCapture,
                BlackState = entity.BlackState,
                WhiteState = entity.WhiteState,
                KoInfo = entity.KoInfo.ToDal(),
                TimeControlId = entity.TimeControl.Id,
                RuleId = entity.Rule.Id,
                BlackPlayerId = entity.BlackPlayer.UserId,
                WhitePlayerId = entity.WhitePlayer.UserId
            };
        }
    }
}
