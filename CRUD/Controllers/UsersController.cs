using CRUD.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CRUD.Data;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDBContext _context;

        public UsersController(UserDBContext context)
        {
            _context = context;
        }

        // GET: api/Users/1
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // PUT: api/Users/1
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            // Validaciones
            if (string.IsNullOrWhiteSpace(updatedUser.FirstName) || updatedUser.FirstName.Length > 50)
            {
                return BadRequest("El primer nombre es obligatorio y debe tener máximo 50 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.MiddleName) && updatedUser.MiddleName.Length > 50)
            {
                return BadRequest("El segundo nombre debe tener máximo 50 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(updatedUser.LastName) || updatedUser.LastName.Length > 50)
            {
                return BadRequest("El primer apellido es obligatorio y debe tener máximo 50 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.SecondLastName) && updatedUser.SecondLastName.Length > 50)
            {
                return BadRequest("El segundo apellido debe tener máximo 50 caracteres.");
            }

            if (updatedUser.BirthDate == default)
            {
                return BadRequest("La fecha de nacimiento es obligatoria.");
            }

            if (updatedUser.Salary <= 0)
            {
                return BadRequest("El sueldo debe ser mayor que 0.");
            }

            // Actualiza las propiedades del usuario
            user.FirstName = updatedUser.FirstName;
            user.MiddleName = updatedUser.MiddleName;
            user.LastName = updatedUser.LastName;
            user.SecondLastName = updatedUser.SecondLastName;
            user.BirthDate = updatedUser.BirthDate;
            user.Salary = updatedUser.Salary;
            user.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(user.FirstName) || user.FirstName.Length > 50)
            {
                return BadRequest("El primer nombre es obligatorio y debe tener máximo 50 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(user.MiddleName) && user.MiddleName.Length > 50)
            {
                return BadRequest("El segundo nombre debe tener máximo 50 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(user.LastName) || user.LastName.Length > 50)
            {
                return BadRequest("El primer apellido es obligatorio y debe tener máximo 50 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(user.SecondLastName) && user.SecondLastName.Length > 50)
            {
                return BadRequest("El segundo apellido debe tener máximo 50 caracteres.");
            }

            if (user.BirthDate == default)
            {
                return BadRequest("La fecha de nacimiento es obligatoria.");
            }

            if (user.Salary <= 0)
            {
                return BadRequest("El sueldo debe ser mayor que 0.");
            }

            // Guarda la fecha de creación
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // DELETE: api/Users/1
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

        // GET: api/Users/search?q=John&page=1&limit=10
        [HttpGet("search")]
        public ActionResult<IEnumerable<User>> SearchUsers(string q, int page = 1, int limit = 10)
        {
            var query = _context.Users
                .Where(u => u.FirstName.Contains(q) || u.LastName.Contains(q))
                .OrderBy(u => u.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

            return query;
        }
    }
}
