
namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly string[] _lines;

    public Day03()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        int total = 0;
        bool currentlyInDigit = false;
        string digit = string.Empty;
        bool nextToSymbol = false;
        for (int i = 0; i < _lines.Length; i++)
        {
            string line = _lines[i];
            for (int j = 0 ; j < line.Length; j++)
            {
                if (char.IsDigit(line[j]))
                {
                    currentlyInDigit = true;
                    digit += line[j];
                    nextToSymbol |= i > 0 && j > 0 && !char.IsDigit(_lines[i-1][j-1]) && _lines[i-1][j-1] != '.';
                    nextToSymbol |= i > 0 && !char.IsDigit(_lines[i-1][j]) && _lines[i-1][j] != '.';
                    nextToSymbol |= i > 0 && j < line.Length - 1 && !char.IsDigit(_lines[i-1][j+1]) && _lines[i-1][j+1] != '.';
                    nextToSymbol |= i < _lines.Length - 1 && j > 0 && !char.IsDigit(_lines[i+1][j-1]) && _lines[i+1][j-1] != '.';
                    nextToSymbol |= i < _lines.Length - 1 && !char.IsDigit(_lines[i+1][j]) && _lines[i+1][j] != '.';
                    nextToSymbol |= i < _lines.Length - 1 && j < line.Length - 1 && !char.IsDigit(_lines[i+1][j+1]) && _lines[i+1][j+1] != '.';
                    nextToSymbol |= j > 0 && !char.IsDigit(_lines[i][j-1]) && _lines[i][j-1] != '.';
                    nextToSymbol |= j < line.Length - 1 && !char.IsDigit(_lines[i][j+1]) && _lines[i][j+1] != '.';
                }
                else 
                {
                    if (currentlyInDigit && nextToSymbol)
                    {
                        total += int.Parse(digit);
                    }

                    currentlyInDigit = false;
                    nextToSymbol = false;
                    digit = "";
                }
            }
        }
        return new ($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        long total = 0;
        string digit = string.Empty;
        for (int i = 0; i < _lines.Length; i++)
        {
            string line = _lines[i];
            for (int j = 0 ; j < line.Length; j++)
            {
                if (line[j] != '*')
                {
                    continue;
                }
                bool tl = i > 0 && j > 0 && char.IsDigit(_lines[i-1][j-1]);
                bool t = i > 0 && char.IsDigit(_lines[i-1][j]);
                bool tr = i >0 && j < line.Length - 1 && char.IsDigit(_lines[i-1][j+1]);

                bool topleft = tl;
                bool top = !tl && t && !tr;
                bool topRight = tr&&!t&&!tl || tr&&t&&!tl || tl&&!t&&tr;
                bool left = j > 0 && char.IsDigit(_lines[i][j-1]);
                bool right = j < line.Length - 1 && char.IsDigit(_lines[i][j+1]);

                bool bl = i < _lines.Length - 1 && j > 0 && char.IsDigit(_lines[i+1][j-1]);
                bool b = i < _lines.Length && char.IsDigit(_lines[i+1][j]);
                bool br = i < _lines.Length && j < line.Length - 1 && char.IsDigit(_lines[i+1][j+1]);

                bool botleft = bl;
                bool bot = !bl && b && !br;
                bool botright = br&&!b&&!bl || br&&b&&!bl || bl&&!b&&br;

                bool[] boolarray = {topleft, top, topRight, left, right, botleft, bot, botright};
                bool twotrue = boolarray.Count(b => b) == 2;

                if (!twotrue) continue;

                int[] intarray = new int[8];
                if (topleft) intarray[0] = extractdigit(_lines, i-1, j-1);
                if (top) intarray[1] = extractdigit(_lines, i -1, j);
                if (topRight) intarray[2] = extractdigit(_lines, i-1, j+1);
                if (left) intarray[3] = extractdigit(_lines, i, j-1);
                if (right) intarray[4] = extractdigit(_lines, i, j+1);
                if (botleft) intarray[5] = extractdigit(_lines, i + 1, j-1);
                if (bot) intarray[6] = extractdigit(_lines, i +1, j);
                if (botright) intarray[7] = extractdigit(_lines, i+1, j+1);

                long gear = intarray.Where(x => x != 0).Aggregate((acc,x) => acc * x);
                total += gear;
            }
        }

        return new ($"{total}");
    }

    private int extractdigit(string[] lines, int i, int j)
    {
        string digit = string.Empty;
        int b = j;
        while (b >= 0 && char.IsDigit(lines[i][b]))
        {
            b--;
        }

        int c = j;
        while (c < lines[i].Length && char.IsDigit(lines[i][c]))
        {
            c++;
        }

        for (int x = b + 1; x < c; x++)
        {
            digit += lines[i][x];
        }

        return int.Parse(digit);
    }
}