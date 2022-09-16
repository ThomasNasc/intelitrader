using Microsoft.EntityFrameworkCore;
namespace Registro.Models
{

    public class UserContext : DbContext

    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public virtual DbSet<User> Usuarios { get; set; }

    }



}




