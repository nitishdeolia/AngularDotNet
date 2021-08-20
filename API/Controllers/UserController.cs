using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context){
            _context= context;
        }

        // api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            var users =await _context.Users.ToListAsync();
            return users;
        }

        // api/users/3
        [HttpGet("id")]
        public async Task<ActionResult<AppUser>> GetUser(int id){
            var user =  await _context.Users.FirstOrDefaultAsync(u => u.id == id);
            if(user == null){
                return null; //no user present yet
            }else return user;
        }
    }
}