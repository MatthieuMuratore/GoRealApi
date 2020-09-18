﻿using System.Linq;
using System.Net;
using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Api;
using D = GoReal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Tools.Security.Token;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using GoReal.Services.Api.Mappers;
using GoReal.Models.Api.DataTransfertObject;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<D.User> _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserRepository<D.User> UserService, ITokenService TokenService)
        {
            _userService = UserService;
            _tokenService = TokenService;
        }

        [HttpGet]
        [AuthRequired(D.Role.SuperAdministrator)]
        public IActionResult Get()
        {
            List<User> users = _userService.Get().Select(x => x.ToClient()).ToList();
            if (users is null)
                return Problem(((int)UserResult.Failed).ToString(), statusCode: (int)HttpStatusCode.NotFound);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User user = _userService.Get(id).ToClient();
            if (user is null)
                return Problem(((int)UserResult.Failed).ToString(), statusCode: (int)HttpStatusCode.NotFound);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            switch (_userService.Update(id, user.ToDal()))
            {
                case UserResult.Update:
                    return Ok();
                case UserResult.GoTagNotUnique:
                    return Problem(((int)UserResult.GoTagNotUnique).ToString(), statusCode: (int)HttpStatusCode.BadRequest);
                case UserResult.EmailNotUnique:
                    return Problem(((int)UserResult.EmailNotUnique).ToString(), statusCode: (int)HttpStatusCode.BadRequest);
                default:
                    break;
            }
            return NotFound();
        }

        [HttpPatch("{id}")]
        [AuthRequired(D.Role.SuperAdministrator)]
        public IActionResult Patch(int id, [FromBody] PatchForm form)
        {
            if(string.Equals(form.Action,"activate"))
            {
                if (_userService.Activate(id)) return Ok();
            }
            else if(string.Equals(form.Action, "ban"))
            {
                if (_userService.Ban(id)) return Ok();

            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromHeader(Name = "Authorization")] string authorization)
        {
            string token = authorization.Substring(7);
            IEnumerable<string> properties = new List<string>() { "Roles" };
            IDictionary<string, string> user = _tokenService.DecodeToken(token, properties);

            D.Role UserRoles = (D.Role)int.Parse(user["Roles"]);

            if (UserRoles.HasFlag(D.Role.SuperAdministrator))
            {
                if (_userService.DeleteAdmin(id)) return Ok();
            }
            else
            {
                if (_userService.Delete(id)) return Ok();
            }

            return NotFound();
        }
    }
}
