
namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly string[] _lines;

    public Day08()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        string directions = _lines[0] + _lines[1];

        Dictionary<string, (string,string)> dict = new();

        for(int i = 3; i < _lines.Length; i++)
        {
            string now = _lines[i].Substring(0,3);
            string left = _lines[i].Substring(7,3);
            string right = _lines[i].Substring(12,3);
            dict[now] = (left,right);
        }

        string current = "AAA";
        int directionIndex = 0;
        int steps = 0;
        while(current != "ZZZ")
        {
            char c = directions[directionIndex];
            if (c == 'L')
            {
                current = dict[current].Item1;
            } else 
            {
                current = dict[current].Item2;
            }

            directionIndex = (directionIndex + 1) % directions.Length;
            steps++;
        }

        return new($"{steps}");
    }

    public override ValueTask<string> Solve_2()
    {
        System.Console.WriteLine(_lines[0]);
        string directions = _lines[0]; 

        Dictionary<string, (string,string)> dict = new();

        for(int i = 2; i < _lines.Length; i++)
        {
            string now = _lines[i].Substring(0,3);
            string left = _lines[i].Substring(7,3);
            string right = _lines[i].Substring(12,3);
            dict[now] = (left,right);
        }

        List<string> current = new();
        foreach(string k in dict.Keys)
        {
            if (k.EndsWith("A"))
            {
                current.Add(k);
            }
        }

        System.Console.WriteLine(current.Count);
        foreach(var a in current) System.Console.WriteLine(a);

        int directionIndex = 0;
        int steps = 0;
        string trueCurrent= "NFA";
        while(true)
        {
            char c = directions[directionIndex];
            if (c == 'L')
            {
                trueCurrent = dict[trueCurrent].Item1;
            } else 
            {
                trueCurrent = dict[trueCurrent].Item2;
            }

            directionIndex = (directionIndex + 1) % directions.Length;
            steps++;

            if (trueCurrent.EndsWith('Z'))
            {
                System.Console.WriteLine($"steps={steps}");
            }

            if (steps > 1000000)
            {
                break;
            }
        }

        // KTA = 14893
        // PLA = 19951
        // LJA = 22199
        // AAA = 16579
        // JXA = 17141
        // NFA = 12083
        return new($"{steps}");
    }

    private bool CheckZ(List<string> current)
    {
        foreach(string c in current)
        {
            if (!c.EndsWith("Z"))
            {
                return false;
            }
        }

        return true;
    }
}