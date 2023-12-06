
namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string[] _lines;

    public Day06()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        int[] times = _lines[0].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        int[] distances = _lines[1].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        long total = 1;
        for (int i = 0 ; i < times.Length; i++)
        {
            int differentWays = 0;
            for (int time = 1; time < times[i]; time++)
            {
                int raceTime = times[i] - time;
                int distance = raceTime * time;

                if (distance <= distances[i])
                {
                    continue;
                }

                differentWays++;
            }

            total *= differentWays;

        }

        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        long time = 35937366;
        long distance = 212206012011044;
        // long time = 71530;
        // long distance = 940200;

        long total = 0;
        for (int i = 1; i < time; i++)
        {
            long raceTime = time - i;
            long raceDistance = raceTime * i;
            if (raceDistance > distance)
            {
                total += 1;
            }
        }

        return new($"{total}");
    }
}