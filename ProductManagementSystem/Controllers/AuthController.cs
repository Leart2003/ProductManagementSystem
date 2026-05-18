using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.Dto;

namespace ProductManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase

    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager )
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }


        //Create
        [HttpPost("register")]

        public async Task<IActionResult>Register(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.EmailAddress);

            if (existingUser != null)
            {
                return BadRequest("Email already exist");
            }

            User user = new User
            {
                UserName = registerDto.EmailAddress,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                Email = registerDto.EmailAddress,
                DateofBirth = registerDto.DateOfBirth,
                Gender = registerDto.Gender,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            return Ok("User registered Succesfully");


        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.EmailAdress);

            if (user == null)
            {
                return Unauthorized("Invalid Email");

            }
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password,isPersistent:true, lockoutOnFailure:false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid email or password");
            }
            return Ok("Login succesfullyx");
        }
        //update

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(UpdateUserDto dto)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

         

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.DateofBirth = dto.DateOfBirth;
            user.Gender = dto.Gender;
                

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Profile updated successfully.");
        }


    }
}
