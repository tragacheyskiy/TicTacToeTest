using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicTacToeTest.Models
{
    public class Player : IEquatable<Player>
    {
        [Key] [MaxLength(36)] public string Token { get; set; }

        public ICollection<Game> Games { get; set; } = new HashSet<Game>();

        public bool Equals(Player other)
        {
            return other != null
                && Equals(other.Token);
        }

        public bool Equals(string otherPlayerToken)
        {
            return otherPlayerToken != null
                && Token.Equals(otherPlayerToken, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return obj != null
                && Equals(obj as Player);
        }

        public override int GetHashCode() => Token.GetHashCode();
    }
}
