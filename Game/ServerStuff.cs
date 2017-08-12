using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Aladdin.Game.Models;

namespace Aladdin.Game
{
    /*
    * Mostly ported from .NET sample
     */
    public class ServerStuff
    {
        private string key;
        private bool trainingMode;
        private uint turns;
        private string map;

        private string playURL;
        public string viewURL { get; private set; }

        public Hero myHero { get; private set; }
        public List<Hero> heroes { get; private set; }

        public int currentTurn { get; private set; }
        public int maxTurns { get; private set; }
        public bool finished { get; private set; }
        public bool errored { get; private set; }
        public string errorText { get; private set; }
        private string serverURL;

        public int BoardSize { get; private set; }
        public List<Tile> Board { get; private set; }

        //if training mode is false, turns and map are ignored8
        public ServerStuff(string key, bool trainingMode, uint turns, string serverURL, string map)
        {
            this.key = key;
            this.trainingMode = trainingMode;
            this.serverURL = serverURL;
            Board = new List<Tile>();

            //the reaons im doing the if statement here is so that i dont have to do it later
            if (trainingMode)
            {
                this.turns = turns;
                this.map = map;
            }
        }

        //initializes a new game, its syncronised
        public async Task CreateGame()
        {
            errored = false;

            string uri;

            if (trainingMode)
            {
                uri = serverURL + "/api/training";
            }
            else
            {
                uri = serverURL + "/api/arena";
            }

            //make the request
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = new TimeSpan(0, 15, 0);
                try
                {
                    var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("key", key),
                new KeyValuePair<string, string>("turns", turns.ToString()),
                new KeyValuePair<string, string>("map", map)
            });
                    var resp = await client.PostAsync(uri, formContent);
                    var data = await resp.Content.ReadAsStringAsync();

                    deserialize(data);
                }
                catch (Exception exception)
                {
                    errored = true;
                }
            }
        }

        private void deserialize(string json)
        {
            var gameResponse = JsonConvert.DeserializeObject<GameResponse>(json);

            playURL = gameResponse.playUrl;
            viewURL = gameResponse.viewUrl;

            myHero = gameResponse.Hero;
            heroes = gameResponse.Game.Heroes;

            currentTurn = gameResponse.Game.Turn;
            maxTurns = gameResponse.Game.MaxTurns;
            finished = gameResponse.Game.Finished;

            createBoard(gameResponse.Game.Board.Size, gameResponse.Game.Board.Tiles);
        }

        public async Task moveHero(string direction)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                try
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("key", key),
                        new KeyValuePair<string, string>("dir", direction)
                    });
                    var resp = await client.PostAsync(playURL, formContent);
                    var data = await resp.Content.ReadAsStringAsync();
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine($"StatusCode {resp.StatusCode} Data: {data}");
                        if (!string.IsNullOrEmpty(data))
                            errored = true;
                    }
                    else
                    {
                        deserialize(data);
                    }
                }
                catch (Exception exception)
                {
                    errored = true;
                }
            }
        }

        private void createBoard(int size, string data)
        {
            BoardSize = size;
            //check to see if the board list is already created, if it is, we just overwrite its values\
            Board.Clear();

            //convert the string to the List<List<Tile>>
            int x = 0;
            int y = 0;
            char[] charData = data.ToCharArray();

            for (int i = 0; i < charData.Length; i += 2)
            {
                var tile = new Tile();
                tile.Pos = new Pos { X = x, Y = y };
                switch (charData[i])
                {
                    case '#':
                        tile.Type = TileType.IMPASSABLE_WOOD;
                        break;
                    case ' ':
                        tile.Type = TileType.FREE;
                        break;
                    case '@':
                        switch (charData[i + 1])
                        {
                            case '1':
                                tile.Type = TileType.HERO_1;
                                break;
                            case '2':
                                tile.Type = TileType.HERO_2;
                                break;
                            case '3':
                                tile.Type = TileType.HERO_3;
                                break;
                            case '4':
                                tile.Type = TileType.HERO_4;
                                break;
                        }
                        break;
                    case '[':
                        tile.Type = TileType.TAVERN;
                        break;
                    case '$':
                        switch (charData[i + 1])
                        {
                            case '-':
                                tile.Type = TileType.GOLD_MINE_NEUTRAL;
                                break;
                            case '1':
                                tile.Type = TileType.GOLD_MINE_1;
                                break;
                            case '2':
                                tile.Type = TileType.GOLD_MINE_2;
                                break;
                            case '3':
                                tile.Type = TileType.GOLD_MINE_3;
                                break;
                            case '4':
                                tile.Type = TileType.GOLD_MINE_4;
                                break;
                        }
                        break;
                }
                Board.Add(tile);

                // Map is reversed
                y++;
                if (y == size)
                {
                    y = 0;
                    x++;
                }
            }
        }
    }
}