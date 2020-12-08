using System.Threading.Tasks;
using TicTacToeTest.Models;

namespace TicTacToeTest.Data
{
    public interface IRepository
    {
        /// <summary>
        /// Adds the <paramref name="player"/> to database and returns its <see cref="Player.Token"/>.
        /// </summary>
        /// <param name="player">Player to be added to database.</param>
        /// <returns>
        /// The <see cref="Player.Token"/> of added <paramref name="player"/>.
        /// </returns>
        Task<string> AddPlayerAsync(Player player);

        /// <summary>
        /// Adds the <paramref name="game"/> to database and returns its <see cref="Game.Id"/>.
        /// </summary>
        /// <param name="game">Game to be added to database.</param>
        /// <returns>
        /// The <see cref="Game.Id"/> of added <paramref name="game"/>.
        /// </returns>
        Task<int> AddGameAsync(Game game);

        /// <summary>
        /// Finds <see cref="Player"/> by its <paramref name="token"/>.
        /// </summary>
        /// <param name="token">The value of the primary key for the <see cref="Player"/> to be found.</param>
        /// <returns>
        /// The <see cref="Player"/> found or <see langword="null"/>.
        /// </returns>
        Task<Player> GetPlayerAsync(string token);

        /// <summary>
        /// Finds <see cref="Game"/> by its <paramref name="gameId"/>.
        /// </summary>
        /// <param name="gameId">The value of the primary key for the <see cref="Game"/> to be found.</param>
        /// <returns>
        /// The <see cref="Game"/> found or <see langword="null"/>.
        /// </returns>
        Task<Game> GetGameAsync(int gameId);

        public Task<int> SaveChangesAsync();
    }
}
