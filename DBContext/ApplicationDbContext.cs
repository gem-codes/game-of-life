using GameOfLifeTestProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GameOfLifeTestProject.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Board> Boards { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
