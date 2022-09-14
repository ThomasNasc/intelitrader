﻿
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Registro1._0.Interfaces;
using Registro1._0.Models;

namespace Registro1._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;


        private readonly IUserService _userService;

        public UsersController(IUserService userService, [Optional] ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult> GetAllUsuarios()
        {
            var users = await _userService.GetUsuarios();


            if (users.Any())
            {
                return Ok(users);


            }
            return NotFound();

        }

        //GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetAsyncUser(string id)
        {
            var _user = await _userService.GetUser(id);
            if (_user.Id == "Usuario Invalido")
            {
                return NotFound();
            }

            return Ok(_user);
        }


        //// PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsyncUser(string id, User user)
        {
            return await _userService.PutUser(id, user);
            


        }


        //// POST: api/Users
        [HttpPost]
        public async Task<ActionResult> PostAsyncUser(User user)
        {
            if (user.firstName == null || user.age == 0)
            {
                return BadRequest();
            }

            var _user = await _userService.PostUser(user);
       

            return CreatedAtAction("GetAsyncUser", new { id = _user.Id }, _user);
        }

        //// DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsyncUser(string id)
        {
            return  await _userService.DeleteUser(id)  ;
        }


    }
}