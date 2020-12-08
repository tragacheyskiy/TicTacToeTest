using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicTacToeTest.Models
{
    public class Game
    {
        public int Id { get; set; }
        [MaxLength(36)] public string CrossToken { get; set; }
        [MaxLength(36)] public string ZeroToken { get; set; }
        [MaxLength(15)] public string Status { get; set; } = Enum.GetName(GameStatus.Starts);

        public IList<Player> Players { get; set; } = new List<Player>();

        public IList<GameMove> GameMoves { get; set; } = new List<GameMove>();
    }
}
