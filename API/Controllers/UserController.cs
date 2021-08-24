using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context){
            _context= context;
        }

        // api/users
        [AllowAnonymous]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            var users =await _context.Users.ToListAsync();
            return users;
        }

        // api/users/3
        [HttpGet("id")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUser(int id){
            var user =  await _context.Users.FirstOrDefaultAsync(u => u.id == id);
            if(user == null){
                return null; //no user present yet
            }else return user;
        }
    }
}