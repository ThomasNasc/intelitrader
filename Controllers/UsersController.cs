using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Registro1._0.Models;

namespace Registro1._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;


        private readonly UserContext _context;

        public UsersController(UserContext context,[Optional] ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsuarios()
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            _logger.LogInformation("##################### LISTA DE USUARIOS COLETADA #####################");
            return await _context.Usuarios.ToListAsync()  ;
            
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
          if (_context.Usuarios == null)
          {
                _logger.LogInformation("##################### DATABASE NÃO ENCONTRADA #####################");
                return NotFound();
          }
            var user = await _context.Usuarios.FindAsync(id);

            if (user == null)
            {
                _logger.LogInformation("##################### USUARIO NAO ENCONTRADO #####################");
                return NotFound();
            }
            _logger.LogInformation("##################### Informacoes do Usuario id: {id} coletadas #####################", id);
            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
    
            var userInFocus = await _context.Usuarios.FindAsync(id);

            if(user.firstName != null)
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

            try
            {
             
                await _context.SaveChangesAsync();
                _logger.LogInformation("##################### DADOS DO USUARIO {id} ATUALIZADOS #####################", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    _logger.LogInformation("##################### USUARIO id: {id} NÃO ENCONTRADO #####################", id);
                    return NotFound();
                   
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var _user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                firstName = user.firstName,
                surName = user.surName,
                age = user.age,
                dateOfCreation = DateTime.Now,
            };

         
          if (_context.Usuarios == null)
          {
              return Problem("Entity set 'UserContext.Usuarios'  is null.");
          }
            _context.Usuarios.Add(_user);
            try
            {
                _logger.LogInformation("##################### USUARIO id: {id} CRIADO #####################", _user.Id);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(_user.Id))
                {
                    _logger.LogInformation("##################### USUARIO id: {id} JA EXISTE #####################", _user.Id);
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = _user.Id }, _user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (_context.Usuarios == null)
            {
                _logger.LogInformation("##################### DATABASE NÃO ENCONTRADA #####################");
                return NotFound();
            }
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                _logger.LogInformation("##################### USUARIO NAO ENCONTRADO #####################");
                return NotFound();
            }

            _context.Usuarios.Remove(user);
            _logger.LogInformation("##################### USUARIO {id} REMOVIDO #####################", id);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
