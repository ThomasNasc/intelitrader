using Microsoft.EntityFrameworkCore;
namespace Registro.Models
{

    public class UserContext : DbContext

    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            if(Database != null)
            {
                Database.EnsureCreated();
            }
    
        }
        public virtual DbSet<User> Usuarios { get; set; }

    }



}




