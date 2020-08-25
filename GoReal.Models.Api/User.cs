﻿using GoReal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Models.Api
{
    public class User
    {
        public int UserId { get; set; }
        public string GoTag { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        public Role Roles { get; set; }
    }
}
