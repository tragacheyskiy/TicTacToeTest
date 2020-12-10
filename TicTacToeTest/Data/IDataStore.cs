using System.Threading.Tasks;
using TicTacToeTest.Models;

namespace TicTacToeTest.Data
{
    public interface IDataStore
    {
        Task<string> AddPlayerAsync(Player player);
        Task<int> AddGameAsync(Game game);
        Task AddLogEntry(LogEntry logEntry);
        Task<Player> GetPlayerAsync(string token);
        Task<Game> GetGameAsync(int gameId);
        Task<int> SaveChangesAsync();
    }
}
