﻿using GoReal.Api.Models;
using D = GoReal.Dal.Entities;

namespace GoReal.Api.Services.Mappers
{ 
    public static class UserMappers
    {
        public static User ToClient(this D.User entity)
        {
            return new User() { 
                UserId = entity.UserId,
                GoTag = entity.GoTag,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Password = entity.Password,
                Email = entity.Email,
                isActive = entity.isActive,
                isBan = entity.isBan,
                Roles = (D.Role)entity.Roles
            };
        }
        public static D.User ToDal(this User entity)
        {
            return new D.User()
            {
                UserId = entity.UserId,
                GoTag = entity.GoTag,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Password = entity.Password,
                Email = entity.Email,
                isActive = entity.isActive,
                isBan = entity.isBan
            };
        }
    }
}
