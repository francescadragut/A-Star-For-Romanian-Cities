// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Collections.Generic;

namespace a_star_romania_graph
{
    class Node
    {
        public Node parent = null;
        public string city = null;
        public double distance = 1000000;

        // public double f = 0;
        public double g = 0;
        public double h = 0;
        public double f = 0;

        public Node()
        {

        }

        public class FDistance
        {
            public double fDistance = 0;

            public FDistance()
            {

            }
        }

    }

    class Program
    {
        public static void AddNeighbourData(List<List<string>> data)
        {
            data.Add(new List<string> { "Arad", "Timisoara", "118" });
            data.Add(new List<string> { "Arad", "Zerind", "75" });
            data.Add(new List<string> { "Arad", "Sibiu", "140" });
            data.Add(new List<string> { "Timisoara", "Lugoj", "111" });
            data.Add(new List<string> { "Zerind", "Oradea", "71" });
            data.Add(new List<string> { "Sibiu", "Oradea", "151" });
            data.Add(new List<string> { "Sibiu", "Fagaras", "99" });
            data.Add(new List<string> { "Sibiu", "Ramnicu Valcea", "80" });
            data.Add(new List<string> { "Lugoj", "Mehadia", "70" });
            data.Add(new List<string> { "Fagaras", "Bucharest", "211" });
            data.Add(new List<string> { "Ramnicu Valcea", "Pitesti", "97" });
            data.Add(new List<string> { "Ramnicu Valcea", "Craiova", "146" });
            data.Add(new List<string> { "Mehadia", "Drobeta", "75" });
            data.Add(new List<string> { "Bucharest", "Urziceni", "85" });
            data.Add(new List<string> { "Bucharest", "Giurgiu", "90" });
            data.Add(new List<string> { "Bucharest", "Pitesti", "101" });
            data.Add(new List<string> { "Pitesti", "Craiova", "138" });
            data.Add(new List<string> { "Craiova", "Drobeta", "120" });
            data.Add(new List<string> { "Urziceni", "Harsova", "98" });
            data.Add(new List<string> { "Urziceni", "Vaslui", "142" });
            data.Add(new List<string> { "Harsova", "Eforie", "86" });
            data.Add(new List<string> { "Vaslui", "Iasi", "92" });
            data.Add(new List<string> { "Iasi", "Neamt", "87" });
        }

        public static Dictionary<string, int> AddStraightDistanceData()
        {
            Dictionary<string, int> straight = new Dictionary<string, int>();
            straight["Arad"] = 366;
            straight["Bucharest"] = 0;
            straight["Craiova"] = 160;
            straight["Drobeta"] = 242;
            straight["Eforie"] = 161;
            straight["Fagaras"] = 176;
            straight["Giurgiu"] = 77;
            straight["Harsova"] = 151;
            straight["Iasi"] = 226;
            straight["Lugoj"] = 244;
            straight["Mehadia"] = 241;
            straight["Neamt"] = 234;
            straight["Oradea"] = 380;
            straight["Pitesti"] = 100;
            straight["Ramnicu Valcea"] = 193;
            straight["Sibiu"] = 253;
            straight["Timisoara"] = 329;
            straight["Urziceni"] = 80;
            straight["Vaslui"] = 199;
            straight["Zerind"] = 374;
            return straight;
        }



        public static List<string> AStar(string startCity, List<List<string>> data, Dictionary<string, int> straight)
        {
            PriorityQueue<Node, double> openQueue = new PriorityQueue<Node, double>();
            PriorityQueue<Node, double> closedQueue = new PriorityQueue<Node, double>();


            Node startNode = new Node();
            startNode.city = startCity;

            startNode.f = 1000000;

            Node endNode = new Node();
            endNode.city = "Bucharest";
            endNode.f = 1000000;

            openQueue.Enqueue(startNode, startNode.f);


            while (openQueue.Count > 0)
            {

                Node currentNode = openQueue.Peek();
                List<List<string>> neighbours = new List<List<string>>();

                int i = 0;

                openQueue.Dequeue();
                closedQueue.Enqueue(currentNode, currentNode.f);


                if (currentNode.city == endNode.city)
                {
                    List<string> path = new List<string>();
                    Node current = currentNode;
                    while (current != null)
                    {
                        path.Add(current.city);
                        current = current.parent;
                    }
                    path.Reverse();
                    return path;
                }

                string currentCity = currentNode.city;
                foreach (List<string> list in data)
                {
                    string city1 = list[0];
                    string city2 = list[1];
                    int distance = Int32.Parse(list[2]);
                    if (currentCity == city1 || currentCity == city2)
                    {
                        neighbours.Add(list);
                    }
                }

                List<Node> children = new List<Node>();
                foreach (List<string> list in neighbours)
                {
                    i = 0;
                    if (list.IndexOf(currentCity) == 0)
                    {
                        i = 1;
                    }
                    Node newNode = new Node();
                    newNode.parent = currentNode;
                    newNode.city = list[i];
                    newNode.distance = Int32.Parse(list[2]);

                    children.Add(newNode);
                }

                foreach (Node child in children)
                {
                    child.g = child.distance + child.parent.g;
                    child.h = straight[child.city];
                    child.f = child.g + child.h;
                    openQueue.Enqueue(child, child.f);
                }
            }
            return null;
        }

        public static void ListPossibleStartCities()
        {
            Dictionary<string, int> straight = AddStraightDistanceData();
            List<string> possibleCities = new List<string>();
            Console.WriteLine("Possible start cities:\n");
            foreach (KeyValuePair<string, int> pair in straight)
            {
                Console.WriteLine("     {0}", pair.Key);
            }
            Console.WriteLine("\n");
        }

        public static void ListRoute(List<string> path, List<List<string>> data)
        {
            for (int i = 0; i < path.Count() - 1; i++)
            {
                int distance_bit = 0;
                foreach (List<string> list in data)
                {
                    if (list.Contains(path[i]) && list.Contains(path[i + 1]))
                    {
                        distance_bit = Int32.Parse(list[2]);
                    }
                }
                Console.WriteLine("     From {0} to {1} --- distance {2}km", path[i], path[i + 1], distance_bit);
            }
        }

        public static double ComputeDistance(List<string> path, List<List<string>> data)
        {
            double total_distance = 0;
            for (int i = 0; i < path.Count() - 1; i++)
            {
                int distance_bit = 0;
                foreach (List<string> list in data)
                {
                    if (list.Contains(path[i]) && list.Contains(path[i + 1]))
                    {
                        distance_bit = Int32.Parse(list[2]);
                        total_distance += distance_bit;
                    }
                }
            }
            return total_distance;
        }


        static void Main(string[] args)
        {
            List<List<string>> data = new List<List<string>>();
            AddNeighbourData(data);

            Dictionary<string, int> straight = AddStraightDistanceData();
            ListPossibleStartCities();


            Console.Write("Start City: ");
            string startCity = Console.ReadLine();

            List<string> path = AStar(startCity, data, straight);

            double total_distance = ComputeDistance(path, data);
            Console.WriteLine("\nThe shortest route from {0} to Bucharest is {1}km.\nThe route is as following:\n", startCity, total_distance);
            ListRoute(path, data);
        }
    }
}