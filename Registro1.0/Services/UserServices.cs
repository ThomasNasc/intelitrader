using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Registro1._0.Controllers;
using Registro1._0.Interfaces;
using Registro1._0.Models;
using System.Net;

namespace Registro1._0.Services
{
    public class UserServices: IUserService
    {
        private readonly UserContext _context;
        private readonly ILogger<UserServices> _logger;
        public UserServices(UserContext context, ILogger<UserServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HttpStatusCode> DeleteUser(string id)
        {
            if (_context == null)
            {
                _logger.LogInformation(" DATABASE NÃO ENCONTRADA ");
                return HttpStatusCode.NotFound;
            }
         
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                _logger.LogInformation(" USUARIO NAO ENCONTRADO ");
                return HttpStatusCode.NotFound;
            }
            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation(" USUARIO {id} REMOVIDO ", id);
            return HttpStatusCode.NoContent;

        }

        public async Task <User> GetUser(string id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            _logger.LogInformation(" Informacoes do Usuario id: {id} coletadas", user.Id);

            return user;
         
        }

        public async Task<List<User>> GetUsuarios()
        {
          
          

            var users = await _context.Usuarios.ToListAsync();
            _logger.LogInformation("LISTA DE USUARIOS COLETADA");
            return users.Select(t=> new User()
           {
               Id = t.Id,
               firstName= t.firstName,
               surName = t.surName,
               age = t.age,
               dateOfCreation =t.dateOfCreation
           }
           ).ToList();
        }

        public async Task<User> PostUser(User user)
        {
            var setUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
                firstName = user.firstName,
                surName = user.surName,
                age = user.age,
                dateOfCreation = DateTime.Now,
            };
      
            _context.Usuarios.Add(setUser);
            await _context.SaveChangesAsync();
            _logger.LogInformation(" USUARIO id: {id} CRIADO ", setUser.Id);
            return setUser;
        }

        public async Task<HttpStatusCode> PutUser(string id, User user)
        {
            var userInFocus = await _context.Usuarios.FindAsync(id);
            if (userInFocus != null)
            {
                if (user.firstName != null)
                {
                    userInFocus.firstName = user.firstName;
                }
                if (user.surName != null)
                {
                    userInFocus.surName = user.surName;
                }
                if (user.age != 0)
                {
                    userInFocus.age = user.age;
                }
                _context.Entry(userInFocus).State = EntityState.Modified;
            }
            try
            {

                await _context.SaveChangesAsync();
                _logger.LogInformation(" DADOS DO USUARIO {id} ATUALIZADOS ", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    _logger.LogInformation(" USUARIO id: {id} NÃO ENCONTRADO ", id);
                    return HttpStatusCode.NotFound;

                }
                else
                {
                    throw;
                }
            }
            return HttpStatusCode.NoContent;
        }

        private bool UserExists(string id)
        {
            return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
