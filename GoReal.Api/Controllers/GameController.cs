﻿using D = GoReal.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using GoReal.Api.Services;
using GoReal.Api.Models;
using GoReal.Common.Exceptions;
using System.Collections.Generic;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService GameService)
        {
            _gameService = GameService;
        }

        [HttpGet]
        public IActionResult GetByUserId([FromQuery] int userId)
        {
            _ = new List<Game>();
            List<Game> games;
            try
            {
                games = _gameService.GetByUserId(userId);
            }
            catch (GameException exception)
            {
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());
            }
            return Ok(games);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _ = new Game();
            Game game;
            try
            {
                game = _gameService.Get(id);
            }
            catch (GameException exception)
            {
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());
            }
            return Ok(game);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Game game)
        {
            _ = new Game();
            Game newGame;
            try
            {
                newGame = _gameService.Create(game);
            }
            catch (GameException exception)
            {
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());
            }
            return Ok(newGame);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] D.Stone newStone)
        {
            _ = new MoveResult();
            MoveResult result;
            try
            {
                result = _gameService.MakeMove(id, newStone);
            }
            catch (GameException execption)
            {
                return Problem(execption.Result.ToString(), statusCode: (int)execption.HttpStatusCode, type: ((int)execption.Result).ToString());
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/pass")]
        public IActionResult Pass(int id, [FromQuery] int userId)
        {
            _ = new MoveResult();
            MoveResult result;
            try
            {
                result = _gameService.Pass(id, userId);
            }
            catch (GameException execption)
            {
                return Problem(execption.Result.ToString(), statusCode: (int)execption.HttpStatusCode, type: ((int)execption.Result).ToString());
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}/resign")]
        public IActionResult Resign(int id, [FromQuery] int userId)
        {
            _ = new MoveResult();
            MoveResult result;
            try
            {
                result = _gameService.Resign(id, userId);
            }
            catch (GameException execption)
            {
                return Problem(execption.Result.ToString(), statusCode: (int)execption.HttpStatusCode, type: ((int)execption.Result).ToString());
            }
            return Ok(result);
        }
    }
}
