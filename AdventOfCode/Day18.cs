
namespace AdventOfCode;

public class Day18 : BaseDay
{
    private readonly string[] _lines;

    public Day18()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    private (int,int) NextSquare(int i, int j, int direction)
    {
        if (direction == 0) return (i, j+1);
        if (direction == 1) return (i+1, j);
        if (direction == 2) return (i, j-1);
        if (direction == 3) return (i-1, j);
        throw new Exception();
    }

    private Dictionary<char, int> directionMapping = new()
    {
        {'U', 3}, {'L', 2}, {'D', 1}, {'R', 0}
    };

    public override ValueTask<string> Solve_1()
    {
        HashSet<(int,int)> seen = new();
        int minI = 0;
        int maxI = 0;
        int minJ = 0;
        int maxJ = 0;
        int i = 0;
        int j = 0;
        seen.Add((0,0));
        foreach (string line in _lines)
        {
            char directionChar = line[0];
            int direction = directionMapping[directionChar];
            int steps = int.Parse(line.Split(" ")[1]);

            while (steps > 0)
            {
                (int nextI, int nextJ) = NextSquare(i, j, direction);
                if (seen.Contains((nextI, nextJ))) System.Console.WriteLine($"{nextI},{nextJ}");
                seen.Add((nextI, nextJ));

                i = nextI;
                j = nextJ;

                minI = Math.Min(minI, i);
                maxI = Math.Max(maxI, i);
                minJ = Math.Min(minJ, j);
                maxJ = Math.Max(maxJ, j);
                steps--; 
            }
        }

        List<(int,int)> inner = new();
        inner.Add((0, 1));
        while (true)
        {
            bool add = false;
            for (int a = 0; a < inner.Count; a++)
            {
                var inn = inner[a];
                var up = NextSquare(inn.Item1, inn.Item2, 3);
                var left = NextSquare(inn.Item1, inn.Item2, 2);
                var down = NextSquare(inn.Item1, inn.Item2, 1);
                var right = NextSquare(inn.Item1, inn.Item2, 0);

                if (!seen.Contains(up) && !inner.Contains(up)) 
                {
                    inner.Add(up);
                    add = true;
                }
                if (!seen.Contains(down) && !inner.Contains(down)) 
                {
                    inner.Add(down);
                    add = true;
                }
                if (!seen.Contains(left) && !inner.Contains(left)) 
                {
                    inner.Add(left);
                    add = true;
                }
                if (!seen.Contains(right) && !inner.Contains(right)) 
                {
                    inner.Add(right);
                    add = true;
                }
            }

            if (!add) break;
        }
        System.Console.WriteLine($"{minI},{maxI},{minJ},{maxJ}");
        return new($"{seen.Count + inner.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}