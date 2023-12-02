
namespace AdventOfCode;

public class BenTemplate : BaseDay
{
    private readonly string[] _lines;

    public BenTemplate()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        System.Console.WriteLine(_lines.Length);
        throw new NotImplementedException();
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}