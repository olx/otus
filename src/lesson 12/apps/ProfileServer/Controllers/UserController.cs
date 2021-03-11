using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace ProfileServer.Controllers
{
	public class UserController : Controller
    {
		private readonly DataContext _context;
		private readonly UserManager<AppUser> _userManager;
		public UserController(DataContext context, UserManager<AppUser> userManager)
		{
			_context = context;
			_userManager = userManager;

		}
		[Route("/api/user/profile")]
		[HttpGet]
		public async Task<ActionResult<Profile>> GetProfile()
		{
			var userId = HttpContext.Request.Headers["X-USER-ID"][0];
			var user = await _context.Users.FindAsync(userId);
			return new Profile { UserName = user.UserName, Email = user.Email, Phone = user.PhoneNumber };
		}
		[Route("/api/user/profile")]
		[HttpPut]
		public async Task<ActionResult<IdentityResult>> UpdateProfile([FromBody]Login login)
		{
			if (string.IsNullOrEmpty(login.DisplayName) || string.IsNullOrEmpty(login.Password))
				return BadRequest();

			var userId = HttpContext.Request.Headers["X-USER-ID"][0];
			var user = await _context.Users.FindAsync(userId);
			var hasher = _userManager.PasswordHasher;
			user.DisplayName = login.DisplayName;
			user.PasswordHash = hasher.HashPassword(user, login.Password);
			await _userManager.UpdateAsync(user);

			return await _userManager.UpdateAsync(user);
		}
	}
}
