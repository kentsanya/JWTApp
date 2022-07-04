using JWTApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using JWTApp.Context;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace JWTApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UsersContext usersContext;

        public HomeController(ILogger<HomeController> logger, UsersContext context)
        {
            _logger = logger;
            usersContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Person person)
        {

            if (ModelState.IsValid) 
            {
                Person? autperson = usersContext.Users.FirstOrDefault(u => (u.Name == person.Name) && (u.Password == person.Password));
                if (autperson == null) return StatusCode(401);
                else 
                {
                    var encodedJwt = CreateJwtToken(autperson.Name);
                    return View("Index", encodedJwt);
                }
            }
            return NotFound(ModelState.ErrorCount);
           
        }
        private string CreateJwtToken(string name) 
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, name) };
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUIDENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(3)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurity(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}