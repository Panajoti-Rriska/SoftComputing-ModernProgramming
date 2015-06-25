using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra.cs
{
    class Map
    {
        Dictionary<string, Dictionary<string, int>> vertices = new Dictionary<string, Dictionary<string, int>>();

        public void add_vertex(string name, Dictionary<string, int> edges)
        {
            vertices[name] = edges;
        }

        public List<string> shortest_path(string start, string finish)
        {
            var previous = new Dictionary<string, string>();
            var distances = new Dictionary<string, int>();
            var nodes = new List<string>();

            List<string> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<string>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;

                    if (distances.ContainsKey(neighbor.Key))
                    {
                        if (alt < distances[neighbor.Key])
                        {
                            distances[neighbor.Key] = alt;
                            previous[neighbor.Key] = smallest;
                        }
                    }
                }
            }

            return path;

        }

        static void Main(string[] args)
        {
            Map map = new Map();

            //adding connections

            map.add_vertex("1", new Dictionary<string, int>() { { "2", 17 }, { "3", 21 }, { "4", 13 } });

            map.add_vertex("2", new Dictionary<string, int>() { { "3", 25 }, { "5", 16 }, { "7", 10 } });

            map.add_vertex("3", new Dictionary<string, int>() { { "6", 20 }, { "8", 10 } });
            map.add_vertex("4", new Dictionary<string, int>() { { "2", 15 }, { "3", 12 }, { "6", 19 }, { "9", 10 } });
            map.add_vertex("5", new Dictionary<string, int>() { { "7", 9 }, { "8", 18 } });
            map.add_vertex("6", new Dictionary<string, int>() { { "8", 17 }, { "9", 21 } });
            map.add_vertex("7", new Dictionary<string, int>() { { "8", 14 }, { "11", 15 } });
            map.add_vertex("8", new Dictionary<string, int>() { { "9", 8 }, { "11", 10 }, { "12", 11 } });
            map.add_vertex("9", new Dictionary<string, int>() { { "10", 20 }, { "12", 13 } });
            map.add_vertex("10", new Dictionary<string, int>() { { "12", 9 }});
            map.add_vertex("11", new Dictionary<string, int>() { { "12", 9 } });

            map.shortest_path("1", "10").ForEach(x => Console.WriteLine(x));

            Console.ReadLine();
            
        }
    }
}
