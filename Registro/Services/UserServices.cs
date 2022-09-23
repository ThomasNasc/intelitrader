using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Registro.Interfaces;
using Registro.Models;
using System.Runtime.InteropServices;

namespace Registro.Services
{
    public class UserServices : IUserService
    {
        private readonly UserContext _context;
        private readonly ILogger<UserServices> _logger;
        public UserServices(UserContext context, [Optional] ILogger<UserServices> logger)
        {
            _context = context;
            _logger = logger;
        }



        public async Task<ObjectResult> DeleteUser(string id)
        {


            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                _logger.LogInformation(" USUARIO NAO ENCONTRADO ");
                return new ObjectResult(null)
                {
                    StatusCode = 404
                };
            }
            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation(" USUARIO {id} REMOVIDO ", id);
            return new ObjectResult(null)
            {
                StatusCode = 204
            };

        }



        public async Task<User> GetUser(string id)
        {




            var user = await _context.Usuarios.FindAsync(id);

            if (user != null)
            {
                _logger.LogInformation(" Informacoes do Usuario id: {id} coletadas", id);
                return user;
            }
            else
            {
                var UserInvalid = new User
                {
                    Id = "Usuario Invalido",
                    firstName = "Invalido",
                    age = 0,
                    dateOfCreation = DateTime.Now
                };
                return UserInvalid;
            }


        }

        public async Task<List<User>> GetUsuarios()
        {

            var users = await _context.Usuarios.ToListAsync();
            _logger.LogInformation("lista de usuarios coletada");
            return users.Select(t => new User()
            {
                Id = t.Id,
                firstName = t.firstName,
                surName = t.surName,
                age = t.age,
                dateOfCreation = t.dateOfCreation
            }
           ).ToList();
        }

        public async Task<User> PostUser(User user)
        {


            var setUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
                firstName = user.firstName,
                surName = user.surName == null ? "": user.surName,
                age = user.age,
                dateOfCreation = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
            };

            _context.Usuarios.Add(setUser);
            await _context.SaveChangesAsync();
            _logger.LogInformation(" USUARIO id: {id} CRIADO ", setUser.Id);
            return setUser;
        }

        public async Task<ObjectResult> PutUser(string id, User user)
        {
            var userInFocus = await _context.Usuarios.FindAsync(id);
            if (userInFocus == null)
            {
                _logger.LogInformation(" USUARIO id: {id} NÃO ENCONTRADO ", id);
                return new ObjectResult(null)
                {
                    StatusCode = 404
                };

            }
            try
            {

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

                await _context.SaveChangesAsync();
                _logger.LogInformation(" DADOS DO USUARIO {id} ATUALIZADOS ", id);
            }
            catch (DbUpdateConcurrencyException)
            {


                if (userInFocus == null)
                {
                    _logger.LogInformation(" USUARIO id: {id} NÃO ENCONTRADO ", id);
                    return new ObjectResult(null)
                    {
                        StatusCode = 404
                    };

                }
                else
                {
                    throw;
                }
            }
            return new ObjectResult(null)
            {
                StatusCode = 204
            };
        }


    }
}
