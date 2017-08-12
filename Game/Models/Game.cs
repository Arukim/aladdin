using System.Collections.Generic;

namespace Aladdin.Game.Models
{
    public class Game
    {
        public string Id { get; set; }
        public int Turn { get; set; }
        public int MaxTurns { get; set; }
        public List<Hero> Heroes { get; set; }
        public Board Board { get; set; }
        public bool Finished { get; set; }
    }
}