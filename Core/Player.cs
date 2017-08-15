using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aladdin.Common;
using Aladdin.Core.Helpers;
using Aladdin.Core.Logic;
using Aladdin.Core.Models;
using Aladdin.Game;
using Aladdin.Game.Models;

namespace Aladdin.Core
{
    public class Player
    {
        public ServerStuff ServerStuff;
        private readonly string _name;

        public string Name => _name;

        public Player(string name, ServerStuff serverStuff)
        {
            _name = name;
            this.ServerStuff = serverStuff;
        }

        //starts everything
        public async Task Run()
        {
            Console.WriteLine($"{Name} is waiting for game");

            await ServerStuff.CreateGame();

            if (ServerStuff.errored == false)
            {
                Console.WriteLine($"{Name} has joined game {ServerStuff.viewURL}");
            }
            else
            {
                return;
            }

            while (ServerStuff.finished == false && ServerStuff.errored == false)
            {
                var dir = MakeMove();
                await ServerStuff.moveHero(dir);

                //Console.WriteLine("completed turn " + serverStuff.currentTurn);
            }

            if (ServerStuff.errored)
            {
                Console.Out.WriteLine($"{Name} error in game: " + ServerStuff.errorText);
            }

            Console.Out.WriteLine($"{Name} has finished game {ServerStuff.viewURL}");

            return;
        }

        protected string MakeMove()
        {
            var watch = new MultiWatch();
            watch.Start("total");

            var pathFinder = new PathFinder(ServerStuff.Board, ServerStuff.BoardSize);

            var tiles = ServerStuff.Board;
            var size = ServerStuff.BoardSize;
            var hero = ServerStuff.myHero.Pos;
            var id = ServerStuff.myHero.Id;

            watch.Measure("pathfinder Init", () => pathFinder.Init(hero));
            //pathFinder.PrintMap();

            Pos target;

            watch.Start("nearest");
            var tavern = FindNearest(tiles, pathFinder, x => x.Type == TileType.TAVERN);
            var neutralMine = FindNearest(tiles, pathFinder, x => x.Type == TileType.GOLD_MINE_NEUTRAL);
            var notMineMine = FindNearest(tiles, pathFinder,
                    x => x.Type >= TileType.GOLD_MINE_NEUTRAL && x.Type != TileType.GOLD_MINE_NEUTRAL + id);
            var enemy = FindNearest(tiles, pathFinder,
                    x => x.Type >= TileType.HERO_1 && x.Type <= TileType.HERO_4 && x.Type != TileType.FREE + id);
            watch.Stop("nearest");

            watch.Start("dist");
            var enemyDist = pathFinder.GetDistance(enemy);
            var tavernDist = pathFinder.GetDistance(tavern);
            watch.Stop("dist");

            //Console.WriteLine($"Tavern: {tavern}, neutral: {neutralMine}, notMine: {notMineMine}");
            var dir = Direction.Stay;
            if (ServerStuff.myHero.Life < 30)
            {
                target = tavern;
            }
            else if (ServerStuff.myHero.Life < 80 && tavern != null && tavernDist < 2)
            {
                target = tavern;
            }
            else if (enemyDist < 2)
            {
                target = enemy;
            }
            else
            {
                target = notMineMine;
            }

            if (target == null)
            {
                //Console.WriteLine("I would stay");
            }
            else
            {
                dir = watch.Measure("getDirection", () => pathFinder.GetDirection(target));
                //Console.WriteLine($"I want to go from {hero.X}:{hero.Y} to {target.X}:{target.Y} using {dir}");
            }
            watch.Stop("total");

            //Console.WriteLine(string.Join(",", watch.Marks.Select(x => $"{x.Item1}:{x.Item2}")));
            return dir;
        }

        protected Pos FindNearest(List<Tile> tiles, PathFinder pathFinder, Func<Tile, bool> cmp)
        {
            var targets = tiles.Where(cmp);

            var target = targets.Select(x => new
            {
                Pos = x.Pos,
                Dist = pathFinder.GetDistance(x.Pos)
            })
                .Where(x => x.Dist > 0)
                .OrderBy(x => x.Dist)
                .FirstOrDefault()?.Pos;
            return target;
        }
    }
}