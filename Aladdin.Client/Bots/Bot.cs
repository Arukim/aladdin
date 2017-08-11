using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alladin.Helpers;
using Alladin.Logic;
using Alladin.Models;

namespace Alladin.Bots
{
    public class Bot
    {
        private ServerStuff serverStuff;

        public Bot(ServerStuff serverStuff)
        {
            this.serverStuff = serverStuff;
        }

        //starts everything
        public async Task Run()
        {
            Console.Out.WriteLine("waiting for game");

            await serverStuff.CreateGame();

            if (serverStuff.errored == false)
            {
                Console.WriteLine(serverStuff.viewURL);
            }

            Random random = new Random();
            while (serverStuff.finished == false && serverStuff.errored == false)
            {
                var dir = MakeMove();
                await serverStuff.moveHero(dir);

                Console.Out.WriteLine("completed turn " + serverStuff.currentTurn);
            }

            if (serverStuff.errored)
            {
                Console.Out.WriteLine("error: " + serverStuff.errorText);
            }

            Console.Out.WriteLine("bot finished");

            Console.WriteLine(serverStuff.viewURL);            
        }

        public string MakeMove()
        {
            var watch = new MultiWatch();
            watch.Start("total");

            var pathFinder = new PathFinder(serverStuff.Board, serverStuff.BoardSize);

            var tiles = serverStuff.Board;
            var size = serverStuff.BoardSize;
            var hero = serverStuff.myHero.Pos;
            var id = serverStuff.myHero.Id;

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
            var enemyDist =  pathFinder.GetDistance(enemy);
            var tavernDist = pathFinder.GetDistance(tavern);
            watch.Stop("dist");

            Console.WriteLine($"Tavern: {tavern}, neutral: {neutralMine}, notMine: {notMineMine}");
            var dir = Direction.Stay;
            if (serverStuff.myHero.Life < 30)
            {
                target = tavern;
            }
            else if (serverStuff.myHero.Life < 80 && tavern != null && tavernDist < 2)
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
                Console.WriteLine("I would stay");
            }
            else
            {
                dir = watch.Measure("getDirection", () => pathFinder.GetDirection(target));
                Console.WriteLine($"I want to go from {hero.X}:{hero.Y} to {target.X}:{target.Y} using {dir}");
            }
            watch.Stop("total");

            Console.WriteLine(string.Join(",", watch.Marks.Select(x => $"{x.Item1}:{x.Item2}")));
            return dir;
        }
        public Pos FindNearest(List<Tile> tiles, PathFinder pathFinder, Func<Tile, bool> cmp)
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