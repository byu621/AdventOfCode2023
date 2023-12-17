
using System.Collections.Generic;

namespace AdventOfCode;

public class Day17 : BaseDay
{
    private readonly string[] _lines;

    public Day17()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    private (int,int) nextSquare(int i, int j, int direction)
    {
        if (direction == 0) return (i, j+1);
        if (direction == 1) return (i+1, j);
        if (direction == 2) return (i, j-1);
        if (direction == 3) return (i-1, j);
        throw new Exception($"direction={direction}");
    }


    private int minSoFar = int.MaxValue;
    private Dictionary<(int, int, int, int), int> memo = new();


    private int Recurse(List<List<int>> board, HashSet<(int,int)> seen, int total, int i, int j, int direction, int consec)
    {
        int height = board.Count;
        int width = board[0].Count;
        
        if (consec > 3) return int.MaxValue;
        if (i < 0 || i >= height || j < 0 || j >= width) return int.MaxValue;
        if (seen.Contains((i, j))) return int.MaxValue;
        int newTotal = total + board[i][j];
        if (newTotal >= minSoFar) return int.MaxValue;
        var memoKey = (i, j, direction, consec);
        if (memo.ContainsKey(memoKey))
        {
            if (memo[memoKey] <= newTotal) return int.MaxValue;
        }

        memo[memoKey] = newTotal;

        if (i == height - 1 && j == width - 1)
        {
            minSoFar = newTotal;
            System.Console.WriteLine($"foundit:{minSoFar}");
            return newTotal;
        }
        seen.Add((i, j));

        (int straightA, int straightB) = nextSquare(i, j, direction);
        int straight = Recurse(board, seen, newTotal, straightA, straightB, direction, consec + 1);

        (int leftA, int leftB) = nextSquare(i, j, (direction + 3) % 4);
        int left = Recurse(board, seen, newTotal, leftA, leftB, (direction + 3) % 4, 1);

        (int rightA, int rightB) = nextSquare(i, j, (direction + 1) % 4);
        int right = Recurse(board, seen, newTotal, rightA, rightB, (direction + 1) % 4, 1);

        seen.Remove((i, j));
        int min = Math.Min(straight, left);
        return Math.Min(min, right);
    }

    public override ValueTask<string> Solve_1()
    { 
        List<List<int>> board = new();
        foreach (string line in _lines)
        {
            board.Add(new());
            foreach (char c in line)
            {
                board.Last().Add(c - '0');
            }
        }

        int left = Recurse(board, new(), -board[0][0], 0, 0, 0, 1);
        int down = Recurse(board, new(), -board[0][0], 0, 0, 0, 1);
        int total = Math.Min(left, down);
        return new($"{total}");
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}