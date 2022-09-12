using Microsoft.AspNetCore.Mvc;
using Registro1._0.Models;
using System.Net;

namespace Registro1._0.Interfaces
{
    public interface IUserService
    {
         Task<List<User>> GetUsuarios();
        Task<User> GetUser(string id);
        Task<HttpStatusCode> PutUser(string id, User user);
       Task<User> PostUser(User user);
        Task<HttpStatusCode> DeleteUser(string id);
    }
}
