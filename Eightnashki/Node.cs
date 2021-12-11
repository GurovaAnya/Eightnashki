using System;
using System.Collections.Generic;
using System.Linq;

namespace Eightnashki
{
    public class Node
    { 
        public string Name { get; }
        public int Level { get; }
        public int Weight { get; private set; }
        public Path? OriginPath { get; }
        private int[,] Table { get; }
        private int[,] IdealTable { get; }
        private Node Parent { get; }
        private int SizeX { get; }
        private int SizeY { get; }


        public Node(int[,] table, int level, Path? originPath, Node parent, int[,] idealTable = null)
        {
            Table = table;
            SizeX = Table.GetLength(0);
            SizeY = Table.GetLength(1);
            IdealTable = idealTable ?? GenerateDefaultIdealTable();
            Level = level;
            OriginPath = originPath;
            Parent = parent;
            Weight = level + CountG();
            
            Name = originPath == null? "" : $"{parent?.Name}-{originPath.ToString()[..1]}-{level}";
        }

        private int[,] GenerateDefaultIdealTable()
        {
            var idealTable = new int[SizeX, SizeY];
            var enumerator = 1;
            for (var j =0; j< SizeY; j++)
            for (var i = 0; i < SizeX; i++)
                idealTable[i, j] = enumerator++;
            idealTable[SizeX - 1, SizeY - 1] = 0;
            return idealTable;
        }

        public int CountG()
        {
            var G = 0;
            for (var i = 0; i < SizeX; i++) 
                for (var j = 0; j < SizeY; j++) 
                    if (IdealTable[i, j] != Table[i, j])
                        G++;
            return G;
        }

        public Point FindO()
        {
            for (var i = 0; i < SizeX; i++) 
            for (var j = 0; j < SizeY; j++)
                if (Table[i, j] == 0)
                    return new Point(i, j);
            throw new Exception();
        }

        public int[,] MakeAChild(Path path, Point point)
        {
            var childTable = (int[,])Table.Clone();
            switch (path)
            {
                case Eightnashki.Path.Up:
                    childTable[point.I, point.J] = Table[point.I, point.J - 1];
                    childTable[point.I, point.J - 1] = 0;
                    return childTable;
                case Eightnashki.Path.Down:
                    childTable[point.I, point.J] = Table[point.I, point.J + 1];
                    childTable[point.I, point.J + 1] = 0;
                    return childTable;
                case Eightnashki.Path.Right:
                    childTable[point.I, point.J] = Table[point.I + 1, point.J];
                    childTable[point.I + 1, point.J] = 0;
                    return childTable;
                case Eightnashki.Path.Left:
                    childTable[point.I, point.J] = Table[point.I - 1, point.J];
                    childTable[point.I - 1, point.J] = 0;
                    return childTable;
                default:
                    throw new ArgumentOutOfRangeException(nameof(path), path, null);
            }
        }

        public List<Node> FindChildren()
        {
            var point = FindO();
            var paths = FindPaths(point);
            if (OriginPath != null)
                paths.Remove(OriginPath.Value.GetOppositePath());
            var children = new List<Node>();
            foreach (var path in paths)
            {
                var childTable = MakeAChild(path, point);
                var child = new Node(childTable, Level + 1, path, this, IdealTable);
                children.Add(child);
            }
            
            this.Weight = Int32.MaxValue; // в этот больше не заходим
            
            return children;
        }

        private List<Path> FindPaths(Point point)
        {
            var paths = new List<Path>()
                {Eightnashki.Path.Down, Eightnashki.Path.Left, Eightnashki.Path.Right, Eightnashki.Path.Up};
            if (point.I == 0)
                paths.Remove(Eightnashki.Path.Left);
            else if (point.I == 2)
                paths.Remove(Eightnashki.Path.Right);
            if (point.J == 0)
                paths.Remove(Eightnashki.Path.Up);
            else if (point.J == 2)
                paths.Remove(Eightnashki.Path.Down);

            return paths.ToList();
        }

        public override string ToString()
        {
            var s = "";
            for (int i = 0; i < Table.GetLength(1); i++) {
                for (int j = 0; j < Table.GetLength(0); j++) {
                    if (j > 0) s += ',';
                    s += Table[j, i];
                }
                s += '\n';

            }

            return s;
        }
    }
}