namespace Alladin.Models
{
    public class Pos
    {
        public int X { get; set; }
        public int Y { get; set; }


        public override string ToString(){
            return $"x: {X} y:{Y}";
        }
    }
}
