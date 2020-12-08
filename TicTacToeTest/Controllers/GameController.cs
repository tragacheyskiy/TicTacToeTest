using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeTest.Data;
using TicTacToeTest.Models;

namespace TicTacToeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        // Хранит незавершённые игры в рамках текущей сесии.
        private static readonly ConcurrentBag<Game> startedGames = new ConcurrentBag<Game>();

        private readonly IRepository gameRepo;

        public GameController(IRepository gameRepo)
        {
            this.gameRepo = gameRepo;
        }

        [HttpGet]
        public async Task<string> GetPlayer()
        {
            var player = new Player { Token = $"{Guid.NewGuid()}" };
            return await gameRepo.AddPlayerAsync(player);
        }

        [HttpGet("{token}")]
        public async Task<ActionResult<int>> GetNewGame(string token)
        {
            Player player = await gameRepo.GetPlayerAsync(token);

            if (player == null)
            {
                return Unauthorized();
            }

            Game game = startedGames.FirstOrDefault(game => game.Players.Any(existingPlayer => existingPlayer.Equals(player)));

            return game == null ? await CreateNewGameAsync(player) : game.Id;
        }

        private async Task<int> CreateNewGameAsync(Player player)
        {
            var game = new Game();
            game.Players.Add(player);
            int gameId = await gameRepo.AddGameAsync(game);
            startedGames.Add(game);
            return gameId;
        }

        [HttpGet("{token}/{gameId}")]
        public async Task<ActionResult<GameField>> GetGameField(string token, int gameId)
        {
            Player player = await gameRepo.GetPlayerAsync(token);

            if (player == null)
            {
                return Unauthorized();
            }

            Game game = startedGames.FirstOrDefault(game => game.Id == gameId);

            if (game == null)
            {
                return NotFound();
            }

            if (game.Players.Any(existingPlayer => existingPlayer.Equals(player)))
            {
                return GetNewGameFiled(game);
            }

            if (game.Players.Count == 2)
            {
                return Forbid();
            }

            game = await AddSecondPlayerToGame(game, player);

            return GetNewGameFiled(game);
        }

        private async Task<Game> AddSecondPlayerToGame(Game game, Player player)
        {
            game = await gameRepo.GetGameAsync(game.Id);
            game.Players.Add(player);
            game.Status = Enum.GetName(GameStatus.Goes);
            await gameRepo.SaveChangesAsync();
            return game;
        }

        private GameField GetNewGameFiled(Game game)
        {
            return new GameField()
            {
                GameId = game.Id,
                Status = game.Status
            };
        }

        // POST api/<GameController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
