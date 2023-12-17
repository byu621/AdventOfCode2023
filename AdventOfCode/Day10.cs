
namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly string[] _lines;

    public Day10()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    private int Recurse(int a, int b, int a2, int b2, int count, List<(int, int)> seen)
    {
        if (a == a2 && b == b2)
        {
            seen.Add((a,b));
            return count;
        }

        seen.Add((a,b));
        seen.Add((a2,b2));

        (int na, int nb) = TakePipe(a,b,seen);
        (int na2, int nb2) = TakePipe(a2,b2,seen);

        return Recurse(na, nb, na2, nb2, count+1, seen);
    }

    private (int, int) TakePipe(int a, int b, List<(int, int)> seen)
    {
        char pipe = _lines[a][b];
        if (pipe == '|')
        {
            if (!seen.Contains((a-1,b))) return (a-1,b);
            if (!seen.Contains((a+1,b))) return (a+1,b);
        } else if (pipe == '-') 
        {
            if (!seen.Contains((a,b-1))) return (a,b-1);
            if (!seen.Contains((a,b+1))) return (a,b+1);
        } else if (pipe == 'L')
        {
            if (!seen.Contains((a-1,b))) return (a-1,b);
            if (!seen.Contains((a,b+1))) return (a,b+1);
        } else if (pipe == 'J')
        {
            if (!seen.Contains((a-1,b))) return (a-1,b);
            if (!seen.Contains((a,b-1))) return (a,b-1);
        } else if (pipe == '7')
        {
            if (!seen.Contains((a+1,b))) return (a+1,b);
            if (!seen.Contains((a,b-1))) return (a,b-1);
        } else if (pipe == 'F')
        {
            if (!seen.Contains((a+1,b))) return (a+1,b);
            if (!seen.Contains((a,b+1))) return (a,b+1);
        } 

        throw new Exception();
    }

    public override ValueTask<string> Solve_1()
    {
        for (int i = 0; i < _lines.Length; i++)
        {
            for (int j = 0; j < _lines[i].Length; j++)
            {
                if (_lines[i][j] == 'S')
                {
                    List<(int,int)> seen = new();
                    seen.Add((i,j));
                    int output = Recurse(i, j - 1, i - 1, j, 1, seen);
                    return new($"{output}");
                }
            }
        }
        throw new NotImplementedException();
    }

    public override ValueTask<string> Solve_2()
    {
        List<(int,int)> seen = new();
        for (int i = 0; i < _lines.Length; i++)
        {
            for (int j = 0; j < _lines[i].Length; j++)
            {
                if (_lines[i][j] == 'S')
                {
                    seen.Add((i,j));
                    _ = Recurse(i, j - 1, i - 1, j, 1, seen);
                    break;
                }
            }
        }
        
        int total = 0;
        for (int i = 1; i < _lines.Length; i++)
        {
            for (int j = 0; j < _lines[0].Length; j++)
            {
                if (seen.Contains((i, j))) continue;

                int count = 0;
                for (int k = j + 1; k < _lines[0].Length; k++)
                {
                    if (!seen.Contains((i, k))) continue;
                    char c = _lines[i][k];
                    if (c == '-') continue;
                    if (c == '|')
                    {
                        count++;
                        continue;
                    } else if (c == '7' || c == 'F')
                    {
                        continue;
                    } else if (c == 'L' || c == 'J' || c == 'S')
                    {
                        count++;
                        continue;
                    }
                }

                total += count % 2 == 1 ? 1 : 0;
                if (count % 2 == 1) Console.WriteLine($"{i},{j}");
            }
        }

        return new($"{total}");
    }
}