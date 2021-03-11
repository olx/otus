using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseContext;
using JwtExtension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Controllers
{
	[AllowAnonymous]
	public class UserController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IJwtGenerator _jwtGenerator;

		public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtGenerator = jwtGenerator;

		}
		
		[Route("/api/user/login")]
		[HttpPost]
		public async Task<ActionResult<User>> LoginAsync([FromBody]Login login)
        {
			var user = await _userManager.FindByEmailAsync(login.Email);
			if (user == null)
				return Unauthorized();

			var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

			if (result.Succeeded)
			{
				return new User
				{
					DisplayName = user.DisplayName,
					Token = _jwtGenerator.GenerateJwtToken(user),
					UserName = user.UserName
				};
			}
			return Unauthorized();
		}

		[Route("/api/user/register")]
		[HttpPost]
		public async Task<ActionResult<IdentityResult>> Register([FromBody] Login login)
		{
			if (string.IsNullOrEmpty(login.DisplayName) || string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
				return BadRequest();

			var user = await _userManager.FindByNameAsync(login.UserName);
			if (user != null)
				return Conflict("User is already exist");

			user = await _userManager.FindByEmailAsync(login.Email);
			if (user != null)
				return Conflict("Email is already exist");

			return await _userManager.CreateAsync(new AppUser { DisplayName = login.DisplayName, UserName = login.UserName, Email = login.Email }, login.Password);
		}
	}
}
