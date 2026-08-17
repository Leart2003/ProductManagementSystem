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


        /// <summary>
        /// Registers a user into database
        /// </summary>
        /// <param name="registerDto"> The user object containing the updated product information.</param>
        /// <returns>If user tried to register with the same emeail user will have a message Email already exist.If user doesnt exist then there will be a message user created succesfully</returns>
        /// 
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
        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="loginDto">User will login with the user information</param>
        /// <returns>If email is invalid there will be a message("invalid email")</returns>
        /// <response code="200">Login succesfully</response>
        /// /// <response code="200">Login succesfully</response>
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
        /// <summary>
        /// Updates information of a user
        /// </summary>
        /// <param name="dto">The updated userDto object information</param>
        /// <returns></returns>
        /// /// <response code="200">User updated succesfully</response>

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
