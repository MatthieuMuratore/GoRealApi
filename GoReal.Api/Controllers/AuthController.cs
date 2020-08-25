﻿using System.Linq;
using System.Net;
using System.Security.Claims;
using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Api;
using D = GoReal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Tools.Security.Token;
using GoReal.Models.Api.Mappers;
using GoReal.Models.Api.Forms;
using GoReal.Api.Infrastrucutre;

namespace GoReal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        IAuthRepository<D.User> _authService;
        IRoleRepository<D.Role> _roleService;
        ITokenService _tokenService;

        private string _roles;

        public AuthController(ITokenService TokenService, IAuthRepository<D.User> AuthService, IRoleRepository<D.Role> RoleService)
        {
            _authService = AuthService;
            _roleService = RoleService;
            _tokenService = TokenService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginForm form)
        {
            User user = _authService.Login(form.Email, form.Password)?.ToClient();
            if (user is null) 
                return Problem("Email or Password error", statusCode: (int)HttpStatusCode.NotFound);

            user.Roles = _roleService.GetUserRole(user.UserId).ToList();
            foreach (D.Role role in user.Roles)
            {
                _roles += $"{role.RoleName} ";
            }
            _roles = _roles.Trim();

            user.Token = _tokenService.EncodeToken(user, user => new Claim[] {  
                new Claim("UserId", user.UserId.ToString()),
                new Claim("GoTag", user.GoTag),
                new Claim("LastName", user.LastName), 
                new Claim("FirstName", user.FirstName), 
                new Claim("Email", user.Email),
                new Claim("Roles", _roles)
            });

            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {
            switch (_authService.Register(user.ToDal()))
            {
                case UserResult.Register:
                    return Ok();
                case UserResult.GoTagNotUnique:
                    return Problem("GoTag already used", statusCode: (int)HttpStatusCode.BadRequest);
                case UserResult.EmailNotUnique:
                    return Problem("Email already used", statusCode: (int)HttpStatusCode.BadRequest);
                default:
                    break;
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [AuthRequired("Administrator,Player")]
        public IActionResult Delete(int id)
        {
            if(_authService.Delete(id))
                return Ok();
            return NotFound();
        }

        [HttpPut("{id}")]
        [AuthRequired("Administrator,Player")]
        public IActionResult Put(int id,[FromBody] User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            switch (_authService.Update(id, user.ToDal()))
            {
                case UserResult.Update:
                    return Ok();
                case UserResult.GoTagNotUnique:
                    return Problem("GoTag already used", statusCode: (int)HttpStatusCode.BadRequest);
                case UserResult.EmailNotUnique:
                    return Problem("Email already used", statusCode: (int)HttpStatusCode.BadRequest);
                default:
                    break;
            }
            return NotFound();
        }
    }
}
