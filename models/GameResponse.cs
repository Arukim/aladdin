
namespace Alladin.Models
{
    public class GameResponse
    {
        public Game Game { get; set; }
        public Hero Hero { get; set; }
        public string Token { get; set; }
        public string viewUrl { get; set; }
        public string playUrl { get; set; }
    }

}