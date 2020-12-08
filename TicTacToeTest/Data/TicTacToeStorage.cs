using Microsoft.EntityFrameworkCore;
using TicTacToeTest.Models;

namespace TicTacToeTest.Data
{
    public class TicTacToeStorage : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameMove> GameMoves { get; set; }

        public TicTacToeStorage(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
