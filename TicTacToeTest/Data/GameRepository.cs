using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TicTacToeTest.Models;

namespace TicTacToeTest.Data
{
    public class GameRepository : IRepository
    {
        private readonly TicTacToeStorage gameStorage;

        public GameRepository(TicTacToeStorage gameStorage)
        {
            this.gameStorage = gameStorage;
        }

        public async Task<int> AddGameAsync(Game game)
        {
            gameStorage.Games.Add(game);
            await gameStorage.SaveChangesAsync();
            return game.Id;
        }

        public async Task<string> AddPlayerAsync(Player player)
        {
            gameStorage.Players.Add(player);
            await gameStorage.SaveChangesAsync();
            return player.Token;
        }

        public async Task<Game> GetGameAsync(int gameId) => await gameStorage.Games.FindAsync(gameId);

        public async Task<Player> GetPlayerAsync(string token) => await gameStorage.Players.FindAsync(token);

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await gameStorage.SaveChangesAsync();
            }
            catch
            {
                return 0;
            }
        }
    }
}
