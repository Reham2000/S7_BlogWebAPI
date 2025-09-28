using Blog.Core.DTos;
using Blog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        public AuthController(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }
        // Register
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(Register model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // set user object
                    User? NewUser = new User
                    {
                        Email = model.Email,
                        UserName = model.UserName

                    };
                    IdentityResult Result = await _userManager.CreateAsync(NewUser, model.Password);
                    if (Result.Succeeded)
                    {
                        return StatusCode(201, new
                        {
                            StatusCode = 201,
                            Message = "User Added Successfully"
                        });
                    }
                    else
                    {
                        foreach (var Error in Result.Errors)
                            ModelState.AddModelError("Create User Error", Error.Description);

                    }
                }
                        
                return BadRequest(ModelState);



            }catch(Exception ex)
            {
                return StatusCode (500, new
                {
                    StatusCode = 500,
                    Message = "An Error ouccered while apliying Api",
                    Error = ex.Message
                });
            }
        }

        // Login
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(Login model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    // validate Email or UserName
                    var User = await _userManager.FindByEmailAsync(model.Email);
                    if(User is not null) // User Found
                    {
                        // validate Password
                        if (await _userManager.CheckPasswordAsync(User, model.Password))
                        {// Valid Password

                            // PayLoad [The Data]
                            var Claims = new List<Claim>();
                            // Custom claim
                            //Claims.Add(new Claim("TokenNumber", "1"));
                            // preDefined Claim
                            Claims.Add(new Claim(ClaimTypes.NameIdentifier,User.Id));
                            Claims.Add(new Claim(ClaimTypes.Name,User.UserName));
                            Claims.Add(new Claim("Email",User.Email));
                            // Token Id
                            Claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
                            // user roles
                            var UserRoles = await _userManager.GetRolesAsync(User);
                            foreach(var Role in UserRoles)
                                Claims.Add(new Claim(ClaimTypes.Role,Role.ToString()));

                            // signingCredentials
                            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
                            var signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
                            
                            var Token = new JwtSecurityToken(
                                    claims: Claims,
                                    expires: DateTime.UtcNow.AddMinutes(10),
                                    issuer: _config["JWT:Issuer"],
                                    audience: _config["JWT:Audience"],
                                    signingCredentials: signingCredentials
                                );

                            var TokenValue = new JwtSecurityTokenHandler().WriteToken(Token);

                            await _userManager.SetAuthenticationTokenAsync(
                                User,"MyToken","AccessToken",TokenValue
                                );



                            return Ok(new
                            {
                                Token = TokenValue,
                                Expiration = Token.ValidTo
                            });
                        }
                        else // Invalid Password
                        {
                           return Unauthorized(new
                           {
                               StatusCode = 401,
                               Message = "This User Not Authorized"
                           });
                        }

                    }
                    else // User Not Found
                    {
                        ModelState.AddModelError("Login Errors", "Not Valid Email");
                    }
                }
                return BadRequest(ModelState);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An Error ouccered while apliying Api",
                    Error = ex.Message
                });
            }
        }

    }
}
