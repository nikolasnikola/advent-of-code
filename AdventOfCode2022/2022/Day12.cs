namespace AdventOfCode2022._2022
{
    internal static class Day12
    {
        internal static void Part2()
        {
            var map = Reader.ReadAsCharMatrix("Day12");

            var startingPositions = GetStartingPositions(map, 'S', 'a');

            List<int> canBeSkipped = new();
            List<int> shortestDistances = new();

            foreach (var startingPosition in startingPositions)
            {
                if (!canBeSkipped.Contains(startingPosition))
                {
                    var shortestPath = FindShortestPath(map, startingPosition);
                    shortestDistances.Add(shortestPath.Item1);
                    canBeSkipped.AddRange(shortestPath.Item2);
                }
            }

            Console.WriteLine("Shortest route: " + shortestDistances.Min());
        }

        internal static void Part1()
        {
            var map = Reader.ReadAsCharMatrix("Day12Test");

            var startingPositions = GetStartingPositions(map, 'S', 'S');

            List<int> canBeSkipped = new();
            List<int> shortestDistances = new();

            foreach (var startingPosition in startingPositions)
            {
                if (!canBeSkipped.Contains(startingPosition))
                {
                    var shortestPath = FindShortestPath(map, startingPosition);
                    shortestDistances.Add(shortestPath.Item1);
                    canBeSkipped.AddRange(shortestPath.Item2);
                }
            }

            Console.WriteLine("Shortest route: " + shortestDistances.Min());
        }

        static (int, List<int>) FindShortestPath(char[,] map, int startPosition)
        {
            var input = map.Cast<char>().ToArray();
            var graphSize = input.Length;

            int[,] graph = new int[graphSize, graphSize];

            int mapRows = map.GetLength(0);
            int mapCols = map.GetLength(1);

            int endPosition = 0;

            for (int i = 0; i < graphSize * graphSize; i++) graph[i % graphSize, i / graphSize] = 0;

            for (int i = 0; i < mapRows; i++)
            {
                for (int j = 0; j < mapCols; j++)
                {
                    var currentValue = map[i, j];

                    if (map[i, j] == 'S')
                    {
                        //startPosition = i * mapCols + j;
                        currentValue = (char)96;
                    }

                    if (map[i, j] == 'E')
                    {
                        endPosition = i * mapCols + j;
                        continue;
                    }

                    var downValue = GetDownValue(map, i, j);
                    var rightValue = GetRightValue(map, i, j);
                    var leftValue = GetLeftValue(map, i, j);
                    var upValue = GetUpValue(map, i, j);

                    if (downValue is not null && downValue.Value.Item1 == 'E')
                    {
                        if (currentValue == 'z')
                        {
                            var currentPosition = i * mapCols + j;
                            var comparePosition = downValue.Value.Item2 * mapCols + downValue.Value.Item3;
                            graph[currentPosition, comparePosition] = 1;
                            continue;
                        }

                    }

                    if (rightValue is not null && rightValue.Value.Item1 == 'E')
                    {
                        if (currentValue == 'z')
                        {
                            var currentPosition = i * mapCols + j;
                            var comparePosition = rightValue.Value.Item2 * mapCols + rightValue.Value.Item3;
                            graph[currentPosition, comparePosition] = 1;
                            continue;
                        }
                    }

                    if (leftValue is not null && leftValue.Value.Item1 == 'E')
                    {
                        if (currentValue == 'z')
                        {
                            var currentPosition = i * mapCols + j;
                            var comparePosition = leftValue.Value.Item2 * mapCols + leftValue.Value.Item3;
                            graph[currentPosition, comparePosition] = 1;
                            continue;
                        }
                    }

                    if (upValue is not null && upValue.Value.Item1 == 'E')
                    {
                        if (currentValue == 'z')
                        {
                            var currentPosition = i * mapCols + j;
                            var comparePosition = upValue.Value.Item2 * mapCols + upValue.Value.Item3;
                            graph[currentPosition, comparePosition] = 1;
                            continue;
                        }
                    }

                    if (downValue is not null && downValue.Value.Item1 - currentValue <= 1 && char.IsLower(downValue.Value.Item1))
                    {
                        var currentPosition = i * mapCols + j;
                        var comparePosition = downValue.Value.Item2 * mapCols + downValue.Value.Item3;
                        graph[currentPosition, comparePosition] = 1;
                    }

                    if (rightValue is not null && rightValue.Value.Item1 - currentValue <= 1 && char.IsLower(rightValue.Value.Item1))
                    {
                        var currentPosition = i * mapCols + j;
                        var comparePosition = rightValue.Value.Item2 * mapCols + rightValue.Value.Item3;
                        graph[currentPosition, comparePosition] = 1;
                    }

                    if (leftValue is not null && leftValue.Value.Item1 - currentValue <= 1 && char.IsLower(leftValue.Value.Item1))
                    {
                        var currentPosition = i * mapCols + j;
                        var comparePosition = leftValue.Value.Item2 * mapCols + leftValue.Value.Item3;
                        graph[currentPosition, comparePosition] = 1;
                    }

                    if (upValue is not null && upValue.Value.Item1 - currentValue <= 1 && char.IsLower(upValue.Value.Item1))
                    {
                        var currentPosition = i * mapCols + j;
                        var comparePosition = upValue.Value.Item2 * mapCols + upValue.Value.Item3;
                        graph[currentPosition, comparePosition] = 1;
                    }
                }
            }

            return Dijkstra(graph, startPosition, graphSize, endPosition, map);
        }

        static List<int> GetStartingPositions(char[,] matrix, char s1, char s2)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            var retList = new List<int>();

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(s1) || matrix[x, y].Equals(s2))
                        retList.Add(x * h + y);
                }
            }

            return retList;
        }

        static (char, int, int)? GetDownValue(char[,] map, int currentRow, int currentColumn)
        {
            var nextRow = currentRow + 1;
            return map.GetLength(0) > nextRow ? (map[nextRow, currentColumn], nextRow, currentColumn) : null;
        }

        static (char, int, int)? GetUpValue(char[,] map, int currentRow, int currentColumn)
        {
            var previousRow = currentRow - 1;
            return previousRow >= 0 ? (map[previousRow, currentColumn], previousRow, currentColumn) : null;
        }

        static (char, int, int)? GetRightValue(char[,] map, int currentRow, int currentColumn)
        {
            var nextColumn = currentColumn + 1;
            return map.GetLength(1) > nextColumn ? (map[currentRow, nextColumn], currentRow, nextColumn) : null;
        }

        static (char, int, int)? GetLeftValue(char[,] map, int currentRow, int currentColumn)
        {
            var previousColumn = currentColumn - 1;
            return previousColumn >= 0 ? (map[currentRow, previousColumn], currentRow, previousColumn) : null;
        }

        // need to optimize
        static (int, List<int>) Dijkstra(int[,] graph, int source, int verticesCount, int endIndex, char[,] map)
        {
            int[] distance = new int[verticesCount];
            bool[] shortestPathTreeSet = new bool[verticesCount];

            List<int> foundedStartPoints = new List<int>();
            bool foundedPath = false;

            for (int i = 0; i < verticesCount; ++i)
            {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            distance[source] = 0;

            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
                shortestPathTreeSet[u] = true;

                for (int v = 0; v < verticesCount; ++v)
                {
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                    {
                        distance[v] = distance[u] + graph[u, v];

                        if (v == endIndex) foundedPath = true;
                 
                    }  
                } 
            }

            if (!foundedPath)
            {
                for (int i = 0; i< distance.Length; i++)
                {
                    if (distance[i] < int.MaxValue && CheckIfStartPoint(map, i))
                    {
                        foundedStartPoints.Add(i);
                    }
                }
            }

            Console.WriteLine("RESULT for " + source +": " + distance[endIndex]);

            return (distance[endIndex], foundedStartPoints);
        }

        static bool CheckIfStartPoint(char[,] map, int number)
        {
            var cols = map.GetLength(1);
            var row = number / cols;
            var col = number % cols;

            return map[row, col] == 'a';
        }

        static int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
        {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (shortestPathTreeSet[v] == false && distance[v] <= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }
    }
}
