namespace Aladdin.Game.Models
{
    public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Elo { get; set; }
        public Pos Pos { get; set; }
        public int Life { get; set; }
        public int Gold { get; set; }
        public int MineCount { get; set; }
        public Pos SpawnPos { get; set; }
        public bool Crashed { get; set; }
    }
}