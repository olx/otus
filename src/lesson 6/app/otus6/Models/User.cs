﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace otus6.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
