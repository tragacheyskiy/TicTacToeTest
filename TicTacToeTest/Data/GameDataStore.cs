using System.Threading.Tasks;
using TicTacToeTest.Models;

namespace TicTacToeTest.Data
{
    internal class GameDataStore : IDataStore
    {
        private readonly TicTacToeDbContext dbContext;

        public GameDataStore(TicTacToeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> AddGameAsync(Game game)
        {
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            return game.Id;
        }

        public Task AddLogEntry(LogEntry logEntry)
        {
            dbContext.Log.Add(logEntry);
            return dbContext.SaveChangesAsync();
        }

        public async Task<string> AddPlayerAsync(Player player)
        {
            dbContext.Players.Add(player);
            await dbContext.SaveChangesAsync();
            return player.Token;
        }

        public async Task<Game> GetGameAsync(int gameId)
        {
            Game game = await dbContext.Games.FindAsync(gameId);
            await dbContext.Entry(game).Collection(game => game.Players).LoadAsync();
            await dbContext.Entry(game).Collection(game => game.GameMoves).LoadAsync();
            return game;
        }

        public async Task<Player> GetPlayerAsync(string token) => await dbContext.Players.FindAsync(token);

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await dbContext.SaveChangesAsync();
            }
            catch
            {
                return 0;
            }
        }
    }
}
