using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeTest.Data;
using TicTacToeTest.Models;
using TicTacToeTest.Services;

namespace TicTacToeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private const string DateTimeTemplate = "MM.dd.yyyy' 'HH:mm:ss:fff";

        private static readonly List<Game> startedGames = new List<Game>();

        private readonly IDataStore gameDataStore;
        private readonly GameGrid gameGrid;

        public GameController(IDataStore gameDataStore)
        {
            this.gameDataStore = gameDataStore;
            gameGrid = new GameGrid();
        }

        #region HttpStatusCodes
        private async Task<NotFoundObjectResult> LogNotFoundAsync(object value, string relativeRequestPath = "", object requestObject = null)
        {
            var logEntry = new LogEntry()
            {
                Message = $"{DateTime.Now.ToString(DateTimeTemplate)}" +
                          $"\nNotFound(404)" +
                          $"\n\tValue: {value}" +
                          $"\n\tRequest: api/game/{relativeRequestPath}" +
                          $"{(requestObject == null ? string.Empty : $"\n\tRequest object: {requestObject}")}"
            };
            await gameDataStore.AddLogEntry(logEntry);
            return NotFound(value);
        }

        private async Task<BadRequestObjectResult> LogBadRequestAsync(object error, string relativeRequestPath = "", object requestObject = null)
        {
            var logEntry = new LogEntry()
            {
                Message = $"{DateTime.Now.ToString(DateTimeTemplate)}" +
                          $"\nBadRequest(400)" +
                          $"\n\tError: {error}" +
                          $"\n\tRequest: api/game/{relativeRequestPath}" +
                          $"{(requestObject == null ? string.Empty : $"\n\tRequest object: {requestObject}")}"
            };
            await gameDataStore.AddLogEntry(logEntry);
            return BadRequest(error);
        }
        #endregion

        #region GET
        [HttpGet]
        public async Task<string> GetPlayer()
        {
            var player = new Player { Token = $"{Guid.NewGuid()}" };
            return await gameDataStore.AddPlayerAsync(player);
        }

        [HttpGet("{token}")]
        public async Task<ActionResult<int>> GetNewGame(string token)
        {
            Player player = await gameDataStore.GetPlayerAsync(token);

            if (player == null)
            {
                return await LogNotFoundAsync("Such player token not found", token);
            }

            Game game = startedGames.FirstOrDefault(game => IsPlayerAttachedToGame(token, game));

            return game == null ? await CreateNewGameAsync(player) : game.Id;
        }

        [HttpGet("{token}/{gameId}")]
        public async Task<ActionResult> GetGame(string token, int gameId)
        {
            Player player = await gameDataStore.GetPlayerAsync(token);

            if (player == null)
            {
                return await LogNotFoundAsync("Such player token not found", $"{token}/{gameId}");
            }

            Game game = GetStartedGame(gameId);

            if (game == null)
            {
                return await LogNotFoundAsync("Such game id not found", $"{token}/{gameId}");
            }

            if (IsPlayerAttachedToGame(token, game))
            {
                return GetNewGameFiled(game);
            }

            if (game.Players.Count == 2)
            {
                return await LogBadRequestAsync("There are no places in the game", $"{token}/{gameId}");
            }

            game = await AddSecondPlayerToGame(game, player);

            return GetNewGameFiled(game);
        }
        #endregion

        #region POST
        [HttpPost]
        public async Task<ActionResult<bool>> Post(GameMoveJson gameMoveJson)
        {
            if (!IsJsonCorrect(gameMoveJson))
            {
                return await LogBadRequestAsync("Incorrect object", requestObject: gameMoveJson);
            }

            Game game = GetStartedGame(gameMoveJson.GameId);
            int indexOfStartedGame = startedGames.IndexOf(game);

            if (game == null)
            {
                return await LogNotFoundAsync("Such game id not found", requestObject: gameMoveJson);
            }

            if (!IsPlayerAttachedToGame(gameMoveJson.PlayerToken, game))
            {
                return await LogBadRequestAsync("Access denied", requestObject: gameMoveJson);
            }

            game = await gameDataStore.GetGameAsync(gameMoveJson.GameId);

            if (game.CrossToken == null)
            {
                game.ZeroToken = game.Players.First(existingPlayer => !existingPlayer.Equals(gameMoveJson.PlayerToken)).Token;
                game.CrossToken = gameMoveJson.PlayerToken;
            }

            string gameStatus = gameGrid.CheckGridAndGetGameStatus(gameMoveJson.Grid);

            GameMove gameMove = new GameMove()
            {
                PlayerToken = gameMoveJson.PlayerToken,
                Grid = gameMoveJson.Grid
            };

            game.GameMoves.Add(gameMove);
            game.Status = gameStatus;

            await gameDataStore.SaveChangesAsync();

            if (IsGameFinished(game))
            {
                startedGames.RemoveAt(indexOfStartedGame);
            }
            else
            {
                startedGames[indexOfStartedGame] = game;
            }

            return true;
        }
        #endregion

        private async Task<int> CreateNewGameAsync(Player player)
        {
            var game = new Game();
            game.Players.Add(player);
            int gameId = await gameDataStore.AddGameAsync(game);
            startedGames.Add(game);
            return gameId;
        }

        private async Task<Game> AddSecondPlayerToGame(Game game, Player player)
        {
            int indexOfStartedGame = startedGames.IndexOf(game);
            game = await gameDataStore.GetGameAsync(game.Id);
            game.Players.Add(player);
            game.Status = GameStatus.Goes;
            await gameDataStore.SaveChangesAsync();
            startedGames[indexOfStartedGame] = game;
            return game;
        }

        private OkObjectResult GetNewGameFiled(Game game)
        {
            return Ok(new { GameId = game.Id, Status = game.Status, Grid = game.GameMoves.LastOrDefault()?.Grid ?? GameGrid.EmptyGrid });
        }

        private bool IsJsonCorrect(GameMoveJson gameMoveJson)
        {
            return gameMoveJson.GameId != 0
                && gameMoveJson.PlayerToken != null
                && gameMoveJson.Grid != GameGrid.EmptyGrid
                && GameGrid.IsGridCorrect(gameMoveJson.Grid);
        }

        private Game GetStartedGame(int gameId)
        {
            return startedGames.FirstOrDefault(game => game.Id == gameId);
        }

        private bool IsPlayerAttachedToGame(string playerToken, Game game)
        {
            return game.Players.Any(existingPlayer => existingPlayer.Equals(playerToken));
        }

        private bool IsGameFinished(Game game)
        {
            return game.Status == GameStatus.CrossWon
                || game.Status == GameStatus.ZeroWon
                || game.Status == GameStatus.Draw;
        }
    }
}
