﻿using Microsoft.EntityFrameworkCore;
namespace Registro1._0.Models
{

        public class UserContext : DbContext

        {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public DbSet<User> Usuarios { get; set; } 

        }
  
    
}

