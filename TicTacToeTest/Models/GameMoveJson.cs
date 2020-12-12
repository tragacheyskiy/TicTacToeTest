using TicTacToeTest.Services;

namespace TicTacToeTest.Models
{
    public class GameMoveJson
    {
        public int GameId { get; set; }
        public string PlayerToken { get; set; }
        public bool IsCapitulation { get; set; }
        public string Grid { get; set; }

        public override string ToString()
        {
            return $"{{\"gameId\":{GameId}," +
                   $"\"playerToken\":\"{PlayerToken}\"," +
                   $"\"isCapitulation\":{IsCapitulation}," +
                   $"\"grid\":\"{Grid}\"}}";
        }
    }
}
