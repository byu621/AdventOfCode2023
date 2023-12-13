
namespace AdventOfCode;

public class Day12 : BaseDay
{
    private readonly string[] _lines;

    public Day12()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    private int Recurse(string baseLine, string line, int index, List<int> pattern)
    {
        if (index == baseLine.Length)
        {
            return PatternMatches(line, pattern) ? 1 : 0;
        }

        if (baseLine[index] != '?')
        {
            line += baseLine[index];
            return Recurse(baseLine, line, index + 1, pattern);
        }

        int a = Recurse(baseLine, line + '.', index + 1, pattern);
        int b = Recurse(baseLine, line + '#', index + 1, pattern);

        return a + b;
    }

    private bool PatternMatches(string line, List<int> pattern)
    {
        int patternIndex = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '#')
            {
                if (patternIndex == pattern.Count) return false;
                if (i + pattern[patternIndex] > line.Length)
                {
                    return false;
                }
                string substring = line.Substring(i, pattern[patternIndex]);
                if (!substring.All(x => x == '#')) return false;

                int nextCharI = i + pattern[patternIndex];
                if (nextCharI < line.Length && line[nextCharI] != '.')
                {
                    return false;
                }
                i += pattern[patternIndex];
                i--;
                patternIndex++;
            } 
        }

        return patternIndex == pattern.Count;
    }

    private int Recurse2(string baseLine, string line, int index, List<int> pattern, int patternIndex, int hashCount)
    {
        if (index == baseLine.Length)
        {
            return PatternMatches(line, pattern) ? 1 : 0;
        }

        if (baseLine[index] == '#')
        {
            if (patternIndex == pattern.Count) return 0;
            if (hashCount > 0)
            {
                hashCount--;
                if (hashCount == 0)
                {
                    if (index + 1 == line.Length) return 1;
                    if (baseLine[index + 1] == '#') return 0;
                    return Recurse2(baseLine, line + "#.", index + 2, pattern, patternIndex, hashCount);
                }

                return Recurse2(baseLine, line + "#", index + 1, pattern, patternIndex, hashCount);
            }

            if (hashCount == 0)
            {
                hashCount = pattern[patternIndex] - 1;
                patternIndex++;
            }

            return Recurse2(baseLine, line + '#', index + 1, pattern, patternIndex, hashCount);
        }

        if (baseLine[index] == '.')
        {
            if (hashCount > 0) return 0;
            return Recurse2(baseLine, line + '.', index + 1, pattern, patternIndex, hashCount);
        }

        if (baseLine[index] == '?')
        {
            if (hashCount > 0)
            {
                hashCount--;
                if (hashCount == 0)
                {
                    if (index + 1 == line.Length) return 1;
                    if (baseLine[index + 1] == '#') return 0;
                    return Recurse2(baseLine, line + "#.", index + 2, pattern, patternIndex, hashCount);
                }

                return Recurse2(baseLine, line + "#", index + 1, pattern, patternIndex, hashCount);
            }

            int a = Recurse2(baseLine, line + '.', index + 1, pattern, patternIndex, hashCount);
            int b = Recurse2(baseLine, line + '#', index + 1, pattern, patternIndex, hashCount);
            return a + b;
        }

        throw new Exception();
    }

    public override ValueTask<string> Solve_1()
    {
        int total = 0;
        foreach(string line in _lines)
        {
            string[] split = line.Split(" ");
            List<int> pattern = split[1].Split(',').Select(int.Parse).ToList();
            string baseLine = split[0];
            total += Recurse(baseLine, "", 0, pattern);
        }
        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
         int total = 0;
         string line = "????.######..#####. 1,6,5";
        // foreach(string line in _lines)
        // {
            string[] split = line.Split(" ");
            List<int> pattern = split[1].Split(',').Select(int.Parse).ToList();
            List<int> expandedPattern = ExpandPattern(pattern);
            string baseLine = split[0];
            string expandedBaseLine = ExpandBaseLine(baseLine);
            total += Recurse2(baseLine, "", 0, pattern, 0, 0);
            
            // if (line == "?#?#?#?#?#?#?#? 1,3,1,6") break;
        // }
        return new($"{total}");
    }

    public string ExpandBaseLine(string baseLine)
    {
        string expanded = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < baseLine.Length; j++)
            {
                expanded += baseLine[j];
            }

            expanded += '?';
        }

        return expanded;
    }


    public List<int> ExpandPattern(List<int> pattern)
    {
        List<int> expanded = new();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < pattern.Count; j++)
            {
                expanded.Add(pattern[j]);
            }
        }

        return expanded;
    }
}