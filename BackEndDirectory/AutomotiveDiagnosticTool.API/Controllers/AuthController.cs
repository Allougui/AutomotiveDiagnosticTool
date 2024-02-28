using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace AutomotiveDiagnosticTool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        
        public AuthController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
       
      


        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserRegistrationDto registrationDto)
        {
            if (string.IsNullOrWhiteSpace(registrationDto.Username) || string.IsNullOrWhiteSpace(registrationDto.Password))
            {
                return BadRequest("Username and password are required.");
            }

            if (_dbContext.Users.Any(u => u.Username == registrationDto.Username))
            {
                return BadRequest("Username already exists.");
            }

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

            // Determine the default role and user type based on input
            string defaultRole = "BasicUser";
            string defaultUserType = "Basic";

            if (registrationDto.UserType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                defaultRole = "Admin";
                defaultUserType = "Admin";
            }

            // Create a new user
            var newUser = new User
            {
                Username = registrationDto.Username,
                Password = hashedPassword,
                Role = defaultRole,
                UserType = defaultUserType
            };

            // Save the user to the database
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();


            return Created("User registered successfully", null);
        }

        [HttpPost("login")]
        public IActionResult AuthenticateUser([FromBody] UserLoginDto loginDto)
        {
            // Validate credentials
            var user = _dbContext.Users.SingleOrDefault(u => u.Username == loginDto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            // Create and return a JWT token
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
        
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("E9R7lmQb4hj3Jy2M9m3dU9P0+oMXXHYHTNl2a9PcfyY="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
