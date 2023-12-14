
using System.Globalization;

namespace AdventOfCode;

public class Day13 : BaseDay
{
    private readonly string[] _lines;

    public Day13()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var list = Construct();
        int total = 0;

        for (int problem = 0; problem < list.Count; problem++)
        {
            (int i, int j) = SolveProblem(list[problem]);

            if (i != -1) total += 100 * (i + 1);
            if (j != -1) total += j + 1;
        }

       return new($"{total}");
    }

    private List<(Dictionary<int,string>, Dictionary<int,string>)> Construct()
    {
        List<(Dictionary<int,string>, Dictionary<int,string>)> list = new();
        for (int k = 0; k < _lines.Length; k++)
        {
            if (k != 0 && _lines[k - 1] != "") continue;

            list.Add((new(), new()));
            var current = list.Last();

            for (int i = k; i < _lines.Length; i++)
            {
                if (_lines[i] == "") break;
                current.Item1[i - k] = _lines[i];

                for (int j = 0; j < _lines[i].Length; j++)
                {
                    if (!current.Item2.ContainsKey(j)) current.Item2[j] = "";
                    current.Item2[j] += _lines[i][j];
                }
            }
        }

        return list;
    }

    private bool IsMirror(int i, Dictionary<int,string> dict)
    {
        int height = dict.Count;
        int row = i;
        while (true)
        {
            int upI = row;
            int downI = 2*i - row + 1;

            if (upI < 0 || downI >= height)
            {
                return true;
            }

            string upString = dict[upI];
            string downString = dict[downI];

            if (upString != downString)
            {
                return false;
            }

            row--;
        }
    }

    private (int,int) SolveProblem((Dictionary<int,string>, Dictionary<int,string>) dicts)
    {
        int height = dicts.Item1.Count;
        int width = dicts.Item2.Count;
        for (int i = 0; i < height - 1; i++)
        {
            bool isHorizontalMirror = IsMirror(i, dicts.Item1);
            if (isHorizontalMirror) return (i, -1);
        }
        for (int j = 0; j < width - 1; j++)
        {
            bool isVerticalMirror = IsMirror(j, dicts.Item2);
            if (isVerticalMirror) return (-1, j);
        }

        // throw new Exception("BEN");
        return (-1,-1);
    }

    private (int,int) SolveProblemIgnoreBefore((Dictionary<int,string>, Dictionary<int,string>) dicts, int beforeI, int beforeJ)
    {
        int height = dicts.Item1.Count;
        int width = dicts.Item2.Count;
        for (int i = 0; i < height - 1; i++)
        {
            bool isHorizontalMirror = IsMirror(i, dicts.Item1);
            if (isHorizontalMirror && i != beforeI) return (i, -1);
        }
        for (int j = 0; j < width - 1; j++)
        {
            bool isVerticalMirror = IsMirror(j, dicts.Item2);
            if (isVerticalMirror && j != beforeJ) return (-1, j);
        }

        // throw new Exception("BEN");
        return (-1,-1);
    }

   // private int SolveJProblem((Dictionary<int,string>, Dictionary<int,string>) dicts)
    // {
    //     int width = dicts.Item2.Count;
    //     for (int j = 0; j < width - 1; j++)
    //     {
    //         bool isVerticalMirror = IsMirror(j, dicts.Item2);
    //         if (isVerticalMirror) return (j);
    //     }
    // }

    private int SolveSmudge(List<(Dictionary<int,string>, Dictionary<int,string>)> list, int problem)
    {
        int beforeI;
        int beforeJ;
        int afterI;
        int afterJ;
        int height = list[problem].Item1.Count;
        int width = list[problem].Item2.Count;
        (beforeI, beforeJ) = SolveProblem(list[problem]);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < list[problem].Item1[i].Length; j++)
            {
                string iString = list[problem].Item1[i];

                char newChar = iString[j] == '#' ? '.' : '#';

                string iReplaceString = ReplaceAt(iString, j, newChar);

                list[problem].Item1[i] = iReplaceString;

                (afterI, _) = SolveProblemIgnoreBefore(list[problem], beforeI, beforeJ);
                
                list[problem].Item1[i] = iString;

                if (afterI == -1)
                {
                    continue;
                }

                if (afterI != beforeI)
                {
                    return 100 * (afterI + 1);
                }

                if (afterI == beforeI)
                {
                    continue;
                }

                throw new Exception("WTF");
            }
        }

        for (int j = 0; j < width; j++)
        {
            for (int i = 0; i < list[problem].Item2[j].Length; i++)
            {
                string jString = list[problem].Item2[j];

                if (jString[i] != '#' && jString[i] != '.') {
                    throw new Exception("wtf");
                }
                char newChar = jString[i] == '#' ? '.' : '#';

                string jReplaceString = ReplaceAt(jString, i, newChar);

                list[problem].Item2[j] = jReplaceString;

                (_, afterJ) = SolveProblemIgnoreBefore(list[problem], beforeI, beforeJ);

                list[problem].Item2[j] = jString;

                if (afterJ == -1)
                {
                    continue;
                }

                if (afterJ != beforeJ)
                {
                    return afterJ + 1;
                }

                if (afterJ == beforeJ)
                {
                    continue;
                }

                throw new Exception("WTF");
            }
        }

        throw new Exception("BEN");
    }

    public override ValueTask<string> Solve_2()
    {
        var list = Construct();
        int total = 0;

        for (int problem = 0; problem < list.Count; problem++)
        {
            System.Console.WriteLine(problem);
            total += SolveSmudge(list, problem);
        }

        return new($"{total}");  
    }

    public static string ReplaceAt(string input, int index, char newChar)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input");
        }
        char[] chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }
}