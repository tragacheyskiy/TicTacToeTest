using TicTacToeTest.Services;

namespace TicTacToeTest.Models
{
    public class GameMoveJson
    {
        public int GameId { get; set; }
        public string PlayerToken { get; set; } = "token";
        public bool IsCapitulation { get; set; } = false;
        public string Grid { get; set; } = GameGrid.EmptyGrid;
    }
}
