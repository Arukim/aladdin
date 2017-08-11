using System;
using System.Collections.Generic;
using System.Linq;
using Alladin.Models;

namespace Alladin.Logic
{
    /*
    * Analyse map, create turn path-map
    **/
    public class PathFinder
    {
        private List<Tile> _tiles;
        private int _size;
        private List<int> _map;

        public PathFinder(List<Tile> tiles, int size)
        {
            _tiles = tiles;
            _size = size;
        }

        public void Init(Pos start)
        {
            _map = _tiles.Select(x => -1).ToList();

            _map[GetIndex(start)] = 1;

            BuildMap();
        }

        public int GetDistance(Pos tgt)
        {
            if(tgt == null)
                return -1;
            var dist = _map[GetIndex(tgt)];
            if(dist == -1)
                return -1;
            return Math.Abs(dist) - 1;
        }

        public string GetDirection(Pos tgt)
        {
            var pos = Math.Abs(_map[GetIndex(tgt)]);
            int coord = 0;

            if (pos == 0 || pos == -1)
                return Direction.Stay;

            Func<int, int, bool> check = (x, y) =>
            {
                coord = GetIndex(tgt.X + x, tgt.Y + y);
                var near = _map[coord];
                if (near == pos - 1)
                {
                    tgt = _tiles[coord].Pos;
                    pos--;
                    return true;
                }
                return false;
            };
            while (true)
            {
                if (tgt.X > 0)
                {
                    if (check(-1, 0))
                    {
                        if (pos == 1)
                            return Direction.South;
                        continue;
                    }
                }

                if (tgt.X < this._size - 1)
                {
                    if (check(1, 0))
                    {
                        if (pos == 1)
                            return Direction.North;
                        continue;
                    }
                }

                if (tgt.Y > 0)
                {
                    if (check(0, -1))
                    {
                        if (pos == 1)
                            return Direction.East;
                        continue;
                    }
                }

                if (tgt.Y < this._size - 1)
                {
                    if (check(0, 1))
                    {
                        if (pos == 1)
                            return Direction.West;
                        continue;
                    }
                }
                Console.WriteLine("No path found");
                return Direction.Stay;
            }
        }

        protected void BuildMap()
        {
            bool changed = true;
            var step = 0;
            Action<int> checkAndSet = (pos) =>
            {
                if (_tiles[pos].Type == TileType.IMPASSABLE_WOOD || _map[pos] != -1)
                    return;
                if (_tiles[pos].Type == TileType.FREE){
                    _map[pos] = step + 1;
                    changed = true;
                }else{
                    _map[pos] = -(step + 1);
                }
            };
            while (changed)
            {
                step++;
                changed = false;
                for (var i = 0; i < _size; i++)
                {
                    for (var j = 0; j < _size; j++)
                    {
                        var idx = GetIndex(i, j);
                        var path = _map[idx];
                        var tile = _tiles[idx];
                        // on first step we checking on current hero tile
                        if (path == step && (tile.Type == TileType.FREE || step == 1))
                        {
                            if (i > 0)
                                checkAndSet(GetIndex(i - 1, j));
                            if (i < this._size - 1)
                                checkAndSet(GetIndex(i + 1, j));
                            if (j > 0)
                                checkAndSet(GetIndex(i, j - 1));
                            if (j < this._size - 1)
                                checkAndSet(GetIndex(i, j + 1));
                        }
                    }
                }
            }
        }

        public void PrintMap()
        {
            for (var i = 0; i < this._size; i++)
            {
                for (var j = 0; j < this._size; j++)
                {
                    var tile = this._map[GetIndex(i, j)];
                    if (tile == -1)
                    {
                        Console.Write("0 ");
                    }
                    else
                    {
                        Console.Write(tile + " ");
                    }
                }
                Console.WriteLine();
            }
        }

        public int GetIndex(Pos p)
        {
            return GetIndex(p.X, p.Y);
        }

        /*
         * Map is reversed
         */
        public int GetIndex(int x, int y)
        {
            return y + x * _size;
        }

    }
}