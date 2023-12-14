
namespace AdventOfCode;

public class Day14 : BaseDay
{
    private readonly string[] _lines;

    public Day14()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    static List<List<char>> RotateMatrix90Degrees(List<List<char>> matrix)
    {
        int rows = matrix.Count;
        int cols = matrix[0].Count;

        List<List<char>> rotatedMatrix = new List<List<char>>(cols);

        for (int i = 0; i < cols; i++)
        {
            rotatedMatrix.Add(new List<char>(rows));
            for (int j = 0; j < rows; j++)
            {
                rotatedMatrix[i].Add(matrix[rows - 1 - j][i]);
            }
        }

        return rotatedMatrix;
    }

    private void TiltNorth(List<List<char>> rows)
    {
        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].Count; j++)
            {
                if (rows[i][j] == 'O')
                {
                    int row = i;
                    while (row >= 1 && rows[row-1][j] == '.')
                    {
                        row--;
                    }

                    if (row < i)
                    {
                        rows[row][j] = 'O';
                        rows[i][j] = '.';
                    }
                }
            }
        }
    }

    private int CalculateLoad(List<List<char>> rows)
    {
        int total = 0;
        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].Count; j++)
            {
                if (rows[i][j] != 'O') continue;
                total += rows.Count - i;
            }

        }

        return total;
    }

    public override ValueTask<string> Solve_1()
    {
        List<List<char>> rows = new();
        foreach(string line in _lines)
        {
            rows.Add(line.ToCharArray().ToList());
        }


        for (int i = 0 ; i < 1000; i++)
        {
            TiltNorth(rows);
            rows = RotateMatrix90Degrees(rows);
            TiltNorth(rows);
            rows = RotateMatrix90Degrees(rows);
            TiltNorth(rows);
            rows = RotateMatrix90Degrees(rows);
            TiltNorth(rows);
            rows = RotateMatrix90Degrees(rows);

            System.Console.WriteLine($"{i}:{CalculateLoad(rows)}");
        }

        int total = CalculateLoad(rows);
        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}