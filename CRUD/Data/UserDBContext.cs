using CRUD.Entity;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Data
{
    public class UserDBContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
