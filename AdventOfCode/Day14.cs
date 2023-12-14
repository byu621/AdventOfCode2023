
namespace AdventOfCode;

public class Day14 : BaseDay
{
    private readonly string[] _lines;

    public Day14()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<List<char>> rows = new();
        foreach(string line in _lines)
        {
            rows.Add(line.ToCharArray().ToList());
        }

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

        int total = 0;
        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < rows[i].Count; j++)
            {
                if (rows[i][j] != 'O') continue;
                total += rows.Count - i;
            }

        }

        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}