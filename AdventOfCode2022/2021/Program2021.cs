// See https://aka.ms/new-console-template for more information

using System.Text;
using AdventOfCode2022;

class COA2021
{
    void Old()
    {
        //Puzzle1();
        //Puzzle2();
        //Puzzle3();
        //Puzzle5();

        Puzzle5();
    }

    void Puzzle1()
    {
        var numbers = Reader.ReadAsIntegerArray("Test1");
        int result = 0;

        for (int i = 1; i < numbers.Count(); i++)
        {
            if (numbers[i] > numbers[i - 1])
            {
                result++;
            }
        }

        Console.WriteLine(result);
    }

    void Puzzle2()
    {
        var numbers = Reader.ReadAsIntegerArray("Test1");
        int result = 0;

        for (int i = 0; i < numbers.Count() - 3; i++)
            if (numbers[i + 1] + numbers[i + 2] + numbers[i + 3] > numbers[i] + numbers[i + 1] + numbers[i + 2])
            {
                result++;
            }
        Console.WriteLine(result);
    }

    void Puzzle3()
    {
        var movements = Reader.ReadAsObjectArray<SubmarineMovement>("Test2");

        var result = movements.Aggregate(
            new
            {
                HorizontalPosition = 0,
                Depth = 0,
            },
            (accumulator, m) => new
            {
                HorizontalPosition = m.Direction == "forward" ? accumulator.HorizontalPosition + m.Units : accumulator.HorizontalPosition,
                Depth = m.Direction == "down" ? accumulator.Depth + m.Units : m.Direction == "up" ? accumulator.Depth - m.Units : accumulator.Depth,
            });

        Console.WriteLine(result.HorizontalPosition * result.Depth);
    }

    void Puzzle4()
    {
        var movements = Reader.ReadAsObjectArray<SubmarineMovement>("Test2");

        var result = movements.Aggregate(
            new
            {
                HorizontalPosition = 0,
                Depth = 0,
                Aim = 0,
            },
            (accumulator, m) => new
            {
                HorizontalPosition = m.Direction == "forward" ? accumulator.HorizontalPosition + m.Units : accumulator.HorizontalPosition,
                Depth = m.Direction == "forward" ? accumulator.Depth + accumulator.Aim * m.Units : accumulator.Depth,
                Aim = m.Direction == "down" ? accumulator.Aim + m.Units : m.Direction == "up" ? accumulator.Aim - m.Units : accumulator.Aim,
            });

        Console.WriteLine(result.HorizontalPosition * result.Depth);
    }

    void Puzzle5()
    {
        var cucumberMap = Reader.ReadAsCharMatrix("Text4");

        var totalSteps = 1;

        while (TryToMakeStep(cucumberMap))
        {
            totalSteps++;
        }

        var s = ArrayToStr(cucumberMap);

        Console.WriteLine(totalSteps);
    }

    bool TryToMakeStep(char[,] matrix)
    {
        var s1 = ArrayToStr(matrix);

        var movedEast = TryToMakeEastFacingStep(matrix);

        var s2 = ArrayToStr(matrix);

        var movedSouth = TryToMakeSouthFacingStep(matrix);

        var s3 = ArrayToStr(matrix);

        return movedEast || movedSouth;
    }

    bool TryToMakeEastFacingStep(char[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);

        bool isAnythingMoved = false;

        for (int i = 0; i < rows; i++)
        {
            bool ignoreNextCol = false;
            for (int j = 0; j < cols; j++)
            {
                if (ignoreNextCol)
                {
                    ignoreNextCol = false;
                    continue;
                }
                if (matrix[i, j] == '>')
                {
                    if (MoveEastIfPossible(matrix, i, j))
                    {
                        isAnythingMoved = true;
                        ignoreNextCol = true;
                    }
                }
            }
            CleanTemporarilyFields(matrix);
        }

        return isAnythingMoved;
    }

    bool MoveEastIfPossible(char[,] matrix, int currentRow, int currentCol)
    {
        var totalCols = matrix.GetLength(1);
        var nextCol = currentCol + 1 == totalCols ? 0 : currentCol + 1;

        if (matrix[currentRow, nextCol] == '.')
        {
            matrix[currentRow, currentCol] = 'T';
            matrix[currentRow, nextCol] = '>';
            return true;
        }

        return false;
    }

    bool TryToMakeSouthFacingStep(char[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);

        bool isAnythingMoved = false;

        for (int j = 0; j < cols; j++)
        {
            bool ignoreNextRow = false;

            for (int i = 0; i < rows; i++)
            {
                if (ignoreNextRow)
                {
                    ignoreNextRow = false;
                    continue;
                }
                if (matrix[i, j] == 'v')
                {
                    if (MoveSouthIfPossible(matrix, i, j))
                    {
                        isAnythingMoved = true;
                        ignoreNextRow = true;
                    }
                }
            }
            CleanTemporarilyFields(matrix);
        }

        return isAnythingMoved;
    }

    bool MoveSouthIfPossible(char[,] matrix, int currentRow, int currentCol)
    {
        var totalRows = matrix.GetLength(0);
        var nextRow = currentRow + 1 == totalRows ? 0 : currentRow + 1;

        if (matrix[nextRow, currentCol] == '.')
        {
            matrix[currentRow, currentCol] = 'T';
            matrix[nextRow, currentCol] = 'v';
            return true;
        }

        return false;
    }

    string ArrayToStr(char[,] a)
    {
        var sb = new StringBuilder(string.Empty);
        var maxI = a.GetLength(0);
        var maxJ = a.GetLength(1);
        for (var i = 0; i < maxI; i++)
        {
            sb.Append("\n{");
            for (var j = 0; j < maxJ; j++)
            {
                sb.Append($"{a[i, j]}");
            }

            sb.Append("}");
        }

        sb.Replace("}", "}").Remove(0, 1);
        return sb.ToString();
    }

    void CleanTemporarilyFields(char[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrix[i, j] == 'T')
                {
                    matrix[i, j] = '.';
                }
            }
        }
    }
}

