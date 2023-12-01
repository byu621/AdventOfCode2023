namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;
    private readonly string[] _lines;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
        _lines = File.ReadAllLines(InputFilePath);
    }

    private (int, int) SetValue(int a, int current)
    {
        int nA = a;
        if (a == -1)
        {
            nA = current;
        }

        return (nA, current);
    }

    public override ValueTask<string> Solve_1()
    {
        int total = 0;
        foreach(string line in _lines)
        {
            char a = default;
            char b = default;
            foreach(char c in line)
            {
                if (char.IsDigit(c))
                {
                    if (a == default)
                    {
                        a = c;
                    }

                    b = c;
                }
            }

            string combined = $"{a}{b}";
            int value = int.Parse(combined);
            total += value;
        }

        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        int total = 0;
        foreach(string line in _lines)
        {
            int a = -1;
            int b = -1;
            int i = 0;
            while (i < line.Length)
            {
                if (i < line.Length - 2) // length 3
                {
                    string substring = line.Substring(i, 3);
                    if (substring == "one")
                    {
                        (a,b) = SetValue(a, 1);
                        i++;
                        continue;
                    } else if (substring == "two")
                    {
                        (a,b) = SetValue(a, 2);
                        i++;
                        continue;
                    } else if (substring == "six")
                    {
                        (a,b) = SetValue(a, 6);
                        i++;
                        continue;
                    }
                }

                if (i < line.Length - 3) // length 4
                {
                    string substring = line.Substring(i, 4);
                    if (substring == "four")
                    {
                        (a,b) = SetValue(a, 4);
                        i++;
                        continue;
                    } else if (substring == "five")
                    {
                        (a,b) = SetValue(a, 5);
                        i++;
                        continue;
                    } else if (substring == "nine")
                    {
                        (a,b) = SetValue(a, 9);
                        i++;
                        continue;
                    }
                }

                if (i < line.Length - 4) // lenght 5
                {
                    string substring = line.Substring(i, 5);
                    if (substring == "three")
                    {
                        (a,b) = SetValue(a, 3);
                        i++;
                        continue;
                    } else if (substring == "seven")
                    {
                        (a,b) = SetValue(a, 7);
                        i++;
                        continue;
                    } else if (substring == "eight")
                    {
                        (a,b) = SetValue(a, 8);
                        i++;
                        continue;
                    }
                }

                char c = line[i];
                if (char.IsDigit(c))
                {
                    (a,b) = SetValue(a, c - '0');
                }

                i++;
            }

            string combined = $"{a}{b}";
            System.Console.WriteLine(combined);
            int value = int.Parse(combined);
            total += value;
        }

        return new($"{total}");
    }
}
