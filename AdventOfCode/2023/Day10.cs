namespace AdventOfCode._2023
{
    internal static class Day10
    {
        internal static void Part1()
        {
            var matrix = Reader.ReadAsCharMatrixWithStartPoint("2023", "Day10", 'S', out var startPosition);

            var directions = FindStartPointConnections(matrix, startPosition);
            var direction1Previous = startPosition;
            var direction2Previous = startPosition;

            var result = 1;

            while(directions.Item1 != directions.Item2)
            {
                var newDireciton1 = GetNextConnection(matrix[directions.Item1.Item1, directions.Item1.Item2], (directions.Item1.Item1, directions.Item1.Item2), direction1Previous);
                var newDirection2 = GetNextConnection(matrix[directions.Item2.Item1, directions.Item2.Item2], (directions.Item2.Item1, directions.Item2.Item2), direction2Previous);

                direction1Previous = (directions.Item1.Item1, directions.Item1.Item2);
                direction2Previous = (directions.Item2.Item1, directions.Item2.Item2);

                directions.Item1 = newDireciton1;
                directions.Item2 = newDirection2;
                result++;
            }

            Console.WriteLine(result);
        }

        internal static void Part2()
        {
            var matrix = Reader.ReadAsCharMatrixWithStartPoint("2023", "Day10", 'S', out var startPosition);

            var direction = FindStartPointConnections(matrix, startPosition).Item1;
            var directionPrevious = startPosition;

            var areaItems = new List<(int, int)>() { direction };


            while (direction != startPosition)
            {
                var newDireciton = GetNextConnection(matrix[direction.Item1, direction.Item2], (direction.Item1, direction.Item2), directionPrevious);

                directionPrevious = (direction.Item1, direction.Item2);

                areaItems.Add(newDireciton);

                direction = newDireciton;
            }

            areaItems.Add(startPosition); // add start to close area

            var boundings = GetBoundings(areaItems);

            var result = 0;
            var insidePoints = new List<(int,int)>();

            for (int k = boundings.Item1; k <= boundings.Item2; k++)
                for (int l = boundings.Item3; l <= boundings.Item4; l++)
                {
                    if (!areaItems.Contains((k, l)) && IsPointInside((k, l), areaItems))
                    {
                        insidePoints.Add((k, l));
                        result++;
                    }
                }

            Console.WriteLine(result);
        }

        private static (int, int) GetNextConnection(char currentItem, (int, int) currentPosition, (int,int) previousConnection)
        {
            return currentItem switch
            {
                '|' => currentPosition.Item1 < previousConnection.Item1 ? (currentPosition.Item1 - 1, currentPosition.Item2) : (currentPosition.Item1 + 1, currentPosition.Item2),
                '-' => currentPosition.Item2 < previousConnection.Item2 ? (currentPosition.Item1, currentPosition.Item2 - 1) : (currentPosition.Item1, currentPosition.Item2 + 1),
                'L' => currentPosition.Item1 != previousConnection.Item1 ? (currentPosition.Item1, currentPosition.Item2 + 1) : (currentPosition.Item1 - 1, currentPosition.Item2),
                'J' => currentPosition.Item1 != previousConnection.Item1 ? (currentPosition.Item1, currentPosition.Item2 - 1) : (currentPosition.Item1 - 1, currentPosition.Item2),
                '7' => currentPosition.Item1 != previousConnection.Item1 ? (currentPosition.Item1, currentPosition.Item2 - 1) : (currentPosition.Item1 + 1, currentPosition.Item2),
                _ => currentPosition.Item1 != previousConnection.Item1 ? (currentPosition.Item1, currentPosition.Item2 + 1) : (currentPosition.Item1 + 1, currentPosition.Item2),
            };
        }

        private static ((int,int), (int,int)) FindStartPointConnections(char[,] matrix, (int,int) startPosition)
        {
            var connections = new List<(int,int)>();

            // top
            if (startPosition.Item1 > 0 && CanConnectOnTop(matrix[startPosition.Item1 - 1, startPosition.Item2]))
                connections.Add((startPosition.Item1 - 1, startPosition.Item2));

            // bottom
            if (startPosition.Item1 < matrix.GetLength(0)-1 && CanConnectOnBottom(matrix[startPosition.Item1+1, startPosition.Item2]))
                connections.Add((startPosition.Item1 +1, startPosition.Item2));

            // left
            if (startPosition.Item2 > 0 && CanConnectOnLeft(matrix[startPosition.Item1, startPosition.Item2-1]))
                connections.Add((startPosition.Item1, startPosition.Item2-1));

            // right
            if (startPosition.Item2 < matrix.GetLength(1) - 1 && CanConnectOnRight(matrix[startPosition.Item1, startPosition.Item2+1]))
                connections.Add((startPosition.Item1, startPosition.Item2+1));

            return (connections[0], connections[1]);
        }

        private static bool CanConnectOnTop(char topItem) => topItem == '|' || topItem == 'F' || topItem == '7';
        private static bool CanConnectOnBottom(char bottomItem) => bottomItem == '|' || bottomItem == 'L' || bottomItem == 'J';
        private static bool CanConnectOnLeft(char leftItem) => leftItem == '-' || leftItem == 'L' || leftItem == 'F';
        private static bool CanConnectOnRight(char rightItem) => rightItem == '-' || rightItem == '7' || rightItem == 'J';

        private static bool IsPointInside((int, int) point, List<(int, int)> area)
        {
            bool isInRing = false;
            int j = area.Count - 1;

            for (int i = 0; i < area.Count; i++)
            {
                if ((area[i].Item2 < point.Item2 && area[j].Item2 >= point.Item2)
                    || (area[j].Item2 < point.Item2 && area[i].Item2 >= point.Item2))
                {
                    if (area[i].Item1
                        + ((point.Item2 - area[i].Item2) / (area[j].Item2 - area[i].Item2) * (area[j].Item1 - area[i].Item1))
                        < point.Item1)
                    {
                        isInRing = !isInRing;
                    }
                }

                j = i;
            }

            return isInRing;
        }

        private static (int, int, int, int) GetBoundings(IEnumerable<(int,int)> positions)
        {
            var result = positions.Aggregate(
                  new
                  {
                      MinRow = int.MaxValue,
                      MaxRow = int.MinValue,
                      MinCol = int.MaxValue,
                      MaxCol = int.MinValue,
                  },
                  (accumulator, p) => new
                  {
                      MinRow = Math.Min(p.Item1, accumulator.MinRow),
                      MaxRow = Math.Max(p.Item1, accumulator.MaxRow),
                      MinCol = Math.Min(p.Item2, accumulator.MinCol),
                      MaxCol = Math.Max(p.Item2, accumulator.MaxCol),
                  });

            return (result.MinRow, result.MaxRow, result.MinCol, result.MaxCol);
        }
    }
}