using Microsoft.EntityFrameworkCore;
using TicTacToeTest.Models;

namespace TicTacToeTest.Data
{
    internal class TicTacToeDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameMove> GameMoves { get; set; }

        public TicTacToeDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
