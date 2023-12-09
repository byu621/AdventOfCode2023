
namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string[] _lines;

    public Day09()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }


    public override ValueTask<string> Solve_1()
    {
        long total = 0;
        foreach(string line in _lines)
        {
            List<List<int>> list = new();
            list.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());
            int listI = 0;
            List<int> current = list[listI];

            while (!current.All(x => x == 0))
            {
                List<int> next = new();
                for (int i = 1; i < current.Count; i++)
                {
                    next.Add(current[i] - current[i-1]);
                }

                listI++;
                list.Add(next);
                current = list[listI];
            }

            long value = 0;
            listI--;
            while(listI >= 0)
            {
                value = value + list[listI].Last();
                listI--;
            }

            total += value;
        }
        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
         long total = 0;
        foreach(string line in _lines)
        {
            List<List<int>> list = new();
            list.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());
            int listI = 0;
            List<int> current = list[listI];

            while (!current.All(x => x == 0))
            {
                List<int> next = new();
                for (int i = 1; i < current.Count; i++)
                {
                    next.Add(current[i] - current[i-1]);
                }

                listI++;
                list.Add(next);
                current = list[listI];
            }

            long value = 0;
            listI--;
            while(listI >= 0)
            {
                value = list[listI].First() - value;
                listI--;
            }

            total += value;
        }
        return new($"{total}");
    }
}