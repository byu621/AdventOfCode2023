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

    private string CreateMemoString(char c, int index, int patternIndex, int hashCount)
    {
        return $"{c},{index},{patternIndex},{hashCount}";
    }

    private long Recurse2(Dictionary<string, long> dict, string baseLine, int index, List<int> pattern, int patternIndex, int hashCount)
    {
        char c = index == baseLine.Length ? 'b' : baseLine[index];
        string memoString = CreateMemoString(c, index, patternIndex, hashCount);
        if (dict.ContainsKey(memoString))
        {
            return dict[memoString];
        }

        if (index == baseLine.Length)
        {
            if (patternIndex != pattern.Count)
            {
                dict[memoString] = 0;
                return 0;
            } 
            
            if (hashCount != 0) {
                dict[memoString] = 0;
                return 0;
            } 

            dict[memoString] = 1;
            return 1;
            
        }
        
        if (baseLine[index] == '#')
        {
            if (hashCount > 0)
            {
                hashCount--;
            } 
            else if (hashCount == 0)
            {
                if (patternIndex == pattern.Count) return 0; // no more patterns
                hashCount = pattern[patternIndex] - 1;
                patternIndex++;
            }

            if (hashCount == 0)
            {
                if (index + 1 == baseLine.Length) return Recurse2(dict, baseLine, index + 1, pattern, patternIndex, 0);
                if (baseLine[index + 1] == '#') return 0;
                return Recurse2(dict, baseLine, index + 2, pattern, patternIndex, 0);
            }

            dict[memoString] = Recurse2(dict, baseLine, index + 1, pattern, patternIndex, hashCount);
            return dict[memoString];
        }

        if (baseLine[index] == '.')
        {
            if (hashCount > 0) return 0;
            dict[memoString] = Recurse2(dict, baseLine, index + 1, pattern, patternIndex, hashCount);
            return dict[memoString];
        }

        if (baseLine[index] == '?')
        {
            if (hashCount > 0)
            {
                string newBaseLine = ReplaceCharAtIndex(baseLine, index, '#');
                dict[memoString] = Recurse2(dict, newBaseLine, index, pattern, patternIndex, hashCount);
                return dict[memoString];
            } 
            
            string newBaseLineA = ReplaceCharAtIndex(baseLine, index, '#');
            long a = Recurse2(dict, newBaseLineA, index, pattern, patternIndex, hashCount);
            string newBaseLineB = ReplaceCharAtIndex(baseLine, index, '.');
            long b = Recurse2(dict, newBaseLineB, index, pattern, patternIndex, hashCount);
            dict[memoString] = a + b;
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
        long total = 0;
        foreach(string line in _lines)
        {
            string[] split = line.Split(" ");
            List<int> pattern = split[1].Split(',').Select(int.Parse).ToList();
            List<int> expandedPattern = ExpandPattern(pattern);
            string baseLine = split[0];
            string expandedBaseLine = ExpandBaseLine(baseLine);
            total += Recurse2(new(), expandedBaseLine, 0, expandedPattern, 0, 0);
        }
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

            if (i!=4) expanded += '?';
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

    static string ReplaceCharAtIndex(string input, int index, char replacement)
    {
        if (index < 0 || index >= input.Length)
        {
            // Index out of bounds, return the original string
            return input;
        }

        char[] charArray = input.ToCharArray();
        charArray[index] = replacement;

        return new string(charArray);
    }
}