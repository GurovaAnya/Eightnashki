using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Eightnashki
{
    class Program
    {
        private static Random rand = new Random();
        private static string Time = DateTime.Now.ToString();
        static void Main(string[] args)
        {
            var currentNode = ReadNodeFromFile();
            var allNodes = new List<Node>();
            allNodes.Add(currentNode);

            while (currentNode.CountG() != 0)
            {
                WriteLine("-----------");
                WriteLine($"Analysing node {currentNode.Name}");
                
                var newChildren = currentNode.FindChildren();
                allNodes.AddRange(newChildren);
                foreach (var child in newChildren)
                {
                    WriteLine($"Go {child.OriginPath} weight={child.Weight} H={child.Level} G={child.CountG()}");
                    WriteLine(child.ToString());
                    WriteLine($"Number of nodes analyzed = {allNodes.Count}");
                }
                currentNode = GetMinimalNode(allNodes);

            }
            WriteLine($"Minimal Node weight = {currentNode.Weight}");
            WriteLine(currentNode.ToString());
            WriteLine($"BestPath = {currentNode.Name}");
            WriteLine($"Number of nodes analyzed = {allNodes.Count}");

        }

        private static Node GenerateRandomNode()
        {
            var table = new int [3, 3];
            var numbersLeft = Enumerable.Range(0, 9).ToList();
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var index = rand.Next(0, numbersLeft.Count() - 1);
                    table[i, j] = numbersLeft[index];
                    numbersLeft.RemoveAt(index);

                }
            }

            return new Node(table, -1, null, null);
        }

        private static Node GenerateNodeFromDoc()
        {
            var table = new int [3, 3]
            {
                {2,1,7},
                {4,8,0},
                {3,5,6}
            };

            return new Node(table, -1, null, null);
        }
        
        private static Node GenerateNodeFromHW()
        {
            var table = new int [,]
            {
                {2,4,7},
                {1,0,5},
                {6,8,3}
            };

            var idealTable = new int[,]
            {
                {1, 8, 7},
                {2, 0, 6},
                {3, 4, 5}
            };

            return new Node(table, -1, null, null, idealTable);
        }
        
        private static Node ReadNodeFromFile()
        {
            var list = File.ReadAllText(@"file.txt").Split('\n');

            var lines = list.ToArray();
            var cells = new int[lines.Length,lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                var row = lines[i].Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < lines.Length; j++)
                    cells[j, i] = int.Parse(row[j]);
            }

            return new Node(cells, 0, null, null);
        }
        

        private static Node GetMinimalNode(List<Node> nodes)
        {
            nodes.Sort((node1, node2) => node1.Weight.CompareTo(node2.Weight));
            return nodes[0];
        }

        private static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

    }
}