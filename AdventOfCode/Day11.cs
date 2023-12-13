
using System.Globalization;

namespace AdventOfCode;

public class Day11: BaseDay
{
    private readonly string[] _lines;

    public Day11()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<int> emptyRows = new();
        List<int> emptyCols = new();

        for (int i = 0; i < _lines.Length; i++)
        {
            if (_lines[i].All(x=>x == '.'))
            {
                emptyRows.Add(i);
            }
        }

        for (int j = 0 ; j < _lines[0].Length; j++)
        {
            bool emptyCol = true;
            for (int i = 0; i < _lines.Length; i++)
            {
                if (_lines[i][j] == '#')
                {
                    emptyCol = false;
                }
            }

            if (emptyCol) emptyCols.Add(j);
        }

        // foreach(var a in emptyRows) System.Console.WriteLine(a);
        // foreach(var a in emptyCols) System.Console.WriteLine(a);
        List<(int,int)> galaxies = new();
        for (int i = 0; i < _lines.Length; i++)
        {
            for (int j = 0 ; j < _lines[0].Length; j++)
            {
                if (_lines[i][j] == '#')
                {
                    galaxies.Add((i, j));
                }
            }
        }

        int total = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i+1; j < galaxies.Count; j++)
            {
                int aI = galaxies[i].Item1;
                int aJ = galaxies[i].Item2;
                int bI = galaxies[j].Item1;
                int bJ = galaxies[j].Item2;

                int minI = Math.Min(aI, bI);
                int maxI = Math.Max(aI, bI);
                int minJ = Math.Min(aJ, bJ);
                int maxJ = Math.Max(aJ, bJ);

                int expandedIDiff = emptyRows.Count(x => x > minI && x < maxI);
                int expandedJDiff = emptyCols.Count(x => x > minJ && x < maxJ);

                int iDiff = expandedIDiff + maxI - minI;
                int jDiff = expandedJDiff + maxJ - minJ;
                total += iDiff + jDiff;
            }
       }

        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<int> emptyRows = new();
        List<int> emptyCols = new();

        for (int i = 0; i < _lines.Length; i++)
        {
            if (_lines[i].All(x=>x == '.'))
            {
                emptyRows.Add(i);
            }
        }

        for (int j = 0 ; j < _lines[0].Length; j++)
        {
            bool emptyCol = true;
            for (int i = 0; i < _lines.Length; i++)
            {
                if (_lines[i][j] == '#')
                {
                    emptyCol = false;
                }
            }

            if (emptyCol) emptyCols.Add(j);
        }

        // foreach(var a in emptyRows) System.Console.WriteLine(a);
        // foreach(var a in emptyCols) System.Console.WriteLine(a);
        List<(int,int)> galaxies = new();
        for (int i = 0; i < _lines.Length; i++)
        {
            for (int j = 0 ; j < _lines[0].Length; j++)
            {
                if (_lines[i][j] == '#')
                {
                    galaxies.Add((i, j));
                }
            }
        }

        long total = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i+1; j < galaxies.Count; j++)
            {
                int aI = galaxies[i].Item1;
                int aJ = galaxies[i].Item2;
                int bI = galaxies[j].Item1;
                int bJ = galaxies[j].Item2;

                int minI = Math.Min(aI, bI);
                int maxI = Math.Max(aI, bI);
                int minJ = Math.Min(aJ, bJ);
                int maxJ = Math.Max(aJ, bJ);

                long star2Diff = 1_000_000;
                long expandedIDiff = emptyRows.Count(x => x > minI && x < maxI) * (star2Diff - 1);
                long expandedJDiff = emptyCols.Count(x => x > minJ && x < maxJ) * (star2Diff - 1);

                long iDiff = expandedIDiff + maxI - minI;
                long jDiff = expandedJDiff + maxJ - minJ;
                total += iDiff + jDiff;
            }
       }

        return new($"{total}");
    }
}