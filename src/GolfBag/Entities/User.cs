﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            // can add custom properties to this class, in addition to those given
        }
    }
}