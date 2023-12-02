
using System.Xml.Schema;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string[] _lines;

    public Day02()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        int total = 0;
        foreach(string line in _lines)
        {
            string[] split = line.Split(":");
            int gameId = int.Parse(split[0].Split(" ")[1]);

            string[] split2 = split[1].Split(";");
            bool valid = true;

            foreach(string s2 in split2)
            {
                string[] split3 = s2.Split(',');

                foreach(string s3 in split3)
                {
                    int count = int.Parse(s3.Trim().Split(" ")[0]);
                    string color = s3.Trim().Split(" ")[1];

                    if (color == "red" && count > 12 || color == "green" && count > 13 || color == "blue" && count > 14)
                    {
                        valid = false;
                    }
                }
            }

            if (valid)
            {
                total += gameId;
            }
        }
        
        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        int total = 0;
        foreach(string line in _lines)
        {
            string[] split = line.Split(":");
            int gameId = int.Parse(split[0].Split(" ")[1]);
            string[] split2 = split[1].Split(";");

            int minRed = 0;
            int minGreen = 0;
            int minBlue = 0;

            foreach(string s2 in split2)
            {
                string[] split3 = s2.Split(',');

                foreach(string s3 in split3)
                {
                    int count = int.Parse(s3.Trim().Split(" ")[0]);
                    string color = s3.Trim().Split(" ")[1];

                    if (color == "red")
                    {
                        minRed = Math.Max(count, minRed);
                    }

                    if (color == "green")
                    {
                        minGreen = Math.Max(count, minGreen);
                    }

                    if (color == "blue")
                    {
                        minBlue = Math.Max(count, minBlue);
                    }
                }
            }

            total += minRed * minBlue * minGreen;
        }
        
        return new($"{total}");
    }
}