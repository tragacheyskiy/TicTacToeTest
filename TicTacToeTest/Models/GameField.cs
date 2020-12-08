using TicTacToeTest.Services;

namespace TicTacToeTest.Models
{
    public class GameField
    {
        public int GameId { get; set; }
        public string Status { get; set; }
        public string Grid { get; set; } = GameGrid.EmptyGrid;
    }
}
