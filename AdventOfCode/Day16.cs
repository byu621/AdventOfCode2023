
namespace AdventOfCode;

public class Day16 : BaseDay
{
    private readonly string[] _lines;

    public Day16()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    private (int,int) NextSquare(int i, int j, int direction)
    {
        if (direction == 0) return (i, j+1);
        if (direction == 1) return (i+1, j);
        if (direction == 2) return (i, j-1);
        if (direction == 3) return (i-1, j);
        throw new Exception();
    }

    public override ValueTask<string> Solve_1()
    {
        List<List<char>> board = new();
        foreach (string line in _lines)
        {
            board.Add(new());
            foreach (char c in line)
            {
                board.Last().Add(c);
            }
        }

        int height = board.Count;
        int width = board[0].Count;
        HashSet<(int,int)> energized = new();
        HashSet<(int,int,int)> seen = new();
        Stack<(int,int,int)> stack = new();
        stack.Push((0,-1,0));

        while (stack.Any())
        {
            var current = stack.Pop();
            if (seen.Contains(current)) continue;
            seen.Add(current);
            energized.Add((current.Item1, current.Item2));
            var nextSquare = NextSquare(current.Item1, current.Item2, current.Item3);
            if (nextSquare.Item1 < 0 || nextSquare.Item1 >= height || nextSquare.Item2 < 0 || nextSquare.Item2 >= width)
            {
                continue;
            }

            var nextChar = board[nextSquare.Item1][nextSquare.Item2];

            // up
            bool upA = nextChar == '.' && current.Item3 == 3;
            bool upB = nextChar == '/' && current.Item3 == 0;
            bool upC = nextChar == '|' && (current.Item3 == 0 || current.Item3 == 2);
            bool upD = nextChar == '\\' && current.Item3 == 2;
            bool upE = nextChar == '|' && current.Item3 == 3;

            // down
            bool downA = nextChar == '.' && current.Item3 == 1;
            bool downB = nextChar == '/' && current.Item3 == 2;
            bool downC = nextChar == '|' && (current.Item3 == 0 || current.Item3 == 2);
            bool downD = nextChar == '\\' && current.Item3 == 0;
            bool downE = nextChar == '|' && current.Item3 == 1;

            // right
            bool rightA = nextChar == '.' && current.Item3 == 0;
            bool rightB = nextChar == '/' && current.Item3 == 3;
            bool rightC = nextChar == '-' && (current.Item3 == 1 || current.Item3 == 3);
            bool rightD = nextChar == '\\' && current.Item3 == 1;
            bool rightE = nextChar == '-' && current.Item3 == 0;

            // left
            bool leftA = nextChar == '.' && current.Item3 == 2;
            bool leftB = nextChar == '/' && current.Item3 == 1;
            bool leftC = nextChar == '-' && (current.Item3 == 1 || current.Item3 == 3);
            bool leftD = nextChar == '\\' && current.Item3 == 3;
            bool leftE = nextChar == '-' && current.Item3 == 2;
            
            if (upA || upB || upC || upD || upE)
            {
                stack.Push((nextSquare.Item1, nextSquare.Item2, 3));
            }

            if (downA || downB || downC || downD || downE)
            {
                stack.Push((nextSquare.Item1, nextSquare.Item2, 1));
            }

            if (rightA || rightB || rightC || rightD || rightE)
            {
                stack.Push((nextSquare.Item1, nextSquare.Item2, 0));
            }

            if (leftA || leftB || leftC || leftD || leftE)
            {
                stack.Push((nextSquare.Item1, nextSquare.Item2, 2));
            }

        }

        energized.Remove((0,-1));
        return new($"{energized.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}