using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var pathFinder = new PathFinder(serverStuff.Board, serverStuff.BoardSize);

            var tiles = serverStuff.Board;
            var size = serverStuff.BoardSize;
            var hero = serverStuff.myHero.Pos;
            var id = serverStuff.myHero.Id;

            pathFinder.Init(hero);
            pathFinder.PrintMap();

            Pos target;



            var tavern = FindNearest(tiles, pathFinder, x => x.Type == TileType.TAVERN);
            var tavernDist = pathFinder.GetDistance(tavern);
            var neutralMine = FindNearest(tiles, pathFinder, x => x.Type == TileType.GOLD_MINE_NEUTRAL);
            var notMineMine = FindNearest(tiles, pathFinder,
            x => x.Type >= TileType.GOLD_MINE_NEUTRAL && x.Type != TileType.GOLD_MINE_NEUTRAL + id);
            Console.WriteLine($"Tavern: {tavern}, neutral: {neutralMine}, notMine: {notMineMine}");
            var dir = Direction.Stay;
            if (serverStuff.myHero.Life < 35)
            {
                target = tavern;
            }
            else if(serverStuff.myHero.Life < 80 && tavern != null && tavernDist < 2)
            {
                target = tavern;
            }else{
                target = notMineMine;
            }

            if (target == null)
            {
                Console.WriteLine("I would stay");
            }
            else
            {
                dir = pathFinder.GetDirection(target);
                Console.WriteLine($"I want to go from {hero.X}:{hero.Y} to {target.X}:{target.Y} using {dir}");
            }

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