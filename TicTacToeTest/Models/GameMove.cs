using System.ComponentModel.DataAnnotations;

namespace TicTacToeTest.Models
{
    public class GameMove
    {
        public int Id { get; set; }
        [MaxLength(19)] public string Grid { get; set; }

        public string PlayerToken { get; set; }
        public Player Player { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
