
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode;

public class Day15 : BaseDay
{
    private readonly string[] _lines;

    public Day15()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        long total = 0;
        string[] inputs = _lines[0].Split(",");
        foreach(string input in inputs)
        {
            total += Hash(input);
        }

        return new($"{total}");
    }

    private long Hash(string input)
    {
        long subTotal = 0;
        foreach(char c in input)
        {
            subTotal += c;
            subTotal *= 17;
            subTotal %= 256;
        }

        return subTotal;
    }

    public override ValueTask<string> Solve_2()
    {
        string[] inputs = _lines[0].Split(",");
        var boxes = new List<(string,int)>[256];
        for (int i = 0; i < 256; i++)
        {
            boxes[i] = new();
        }
        foreach(string input in inputs)
        {
            if (input.Contains('='))
            {
                string label = input.Split('=')[0];
                long boxIndex = Hash(label);
                int focalLength = int.Parse(input.Split('=')[1]);

                List<(string,int)> box = boxes[boxIndex];
                if (box.Any(x => x.Item1 == label))
                {
                    var tupleToUpdate = box.First(t => t.Item1 == label);
                    int index = box.IndexOf(tupleToUpdate);
                    box[index] = (label, focalLength);
                } else {
                    box.Add((label, focalLength));
                }
            } else {
                string label = input.Split('-')[0];
                long boxIndex = Hash(label);
                List<(string,int)> box = boxes[boxIndex];

                if (box.Any(x => x.Item1 == label))
                {
                    var item = box.Where(x => x.Item1 == label).First();
                    box.Remove(item);
                }
            }

        }

        long total = 0;
        for (int i = 0; i < 256; i++)
        {
            long subTotal = 0;
            var box = boxes[i];
            for (int j = 0; j <  box.Count; j++)
            {
                subTotal += (1+i) * (1+j) * box[j].Item2;
            }

            total += subTotal;
        }

        return new($"{total}");
    }
}