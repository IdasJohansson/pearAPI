using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using pearAPI.Database;
using pearAPI.Models;

namespace pearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PearDbContext _context;

        public UserController(PearDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'PearDbContext.Users'  is null.");
            }
            // When posting a new user, the password becomes hashed and stored as hashed in the db. 
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        private bool UserExists(Guid id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Tar in ett User object kollar om användarnamnet finns och jämför lösenordet med det hashade lösenordet. 
        [HttpPost("CheckLogin")]
        public async Task<bool> CheckLogin(User user)
        {
            var validUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            if (validUser != null)
            {
                bool validPassword = BCrypt.Net.BCrypt.Verify(user.Password, validUser.Password);
                return validPassword;
            }
            else
            {
                return false;
            }
        }

    }
}
