
using Spectre.Console.Rendering;

namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly string[] _lines;

    public Day04()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        int total = 0;
        foreach(string line in _lines)
        {
            int match = 0;
            string[] split = line.Split(":");
            string[] split2 = split[1].Trim().Split('|');
            string[] winning = split2[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] yours = split2[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach(string your in yours)
            {
                if (winning.Contains(your))
                {
                    match++;
                }
            }

            int score = match == 0 ? 0 : (int)Math.Pow(2, match -1);
            total += score;
        }
        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        int total = 0;
        Dictionary<int, int> dictionary = new();
        foreach(string line in _lines)
        {
            int match = 0;
            string[] split = line.Split(":");
            int cardId = int.Parse(split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]);
            string[] split2 = split[1].Trim().Split('|');
            string[] winning = split2[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] yours = split2[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach(string your in yours)
            {
                if (winning.Contains(your))
                {
                    match++;
                }
            }

            int copies = dictionary.ContainsKey(cardId) ? dictionary[cardId] : 0;
            int realcopies = copies + 1;

            for (int i = 1; i <= match; i++)
            {
                int existingcopies = dictionary.ContainsKey(cardId + i) ? dictionary[cardId + i] : 0;
                dictionary[cardId + i] = existingcopies + realcopies;
            }

            total += realcopies;
        }
        return new($"{total}");
    }
}