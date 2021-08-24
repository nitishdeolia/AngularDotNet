using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using API.TokenServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(ApplicationDbContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }



        [HttpPost("register")]
        public async Task<ActionResult<JwtToken>> Register(RegisterUser regUser)
        {

            if (await UserExists(regUser.UserName)) return BadRequest("User already exists");
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = regUser.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(regUser.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            return new JwtToken{
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginUser lnuser)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == lnuser.UserName);
            if (user == null) return Unauthorized("Invalid UserName");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(lnuser.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return user;
        }
    }
}