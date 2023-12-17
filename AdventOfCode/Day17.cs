
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


    private int minSoFar = 913;
    private Dictionary<string, int> memo = new();


    private int Recurse2(List<List<int>> board, int total, int i, int j, int direction, int consec)
    {
        int height = board.Count;
        int width = board[0].Count;
        
        if (consec > 10) return int.MaxValue;
        if (i < 0 || i >= height || j < 0 || j >= width) return int.MaxValue;
        int newTotal = total + board[i][j];
        if (newTotal >= minSoFar) return int.MaxValue;
        string memoKey = $"${i},{j},{direction},{consec}";
        if (memo.TryGetValue(memoKey, out int value))
        {
            if (value <= newTotal) return int.MaxValue;
        }

        memo[memoKey] = newTotal;

        if (i == height - 1 && j == width - 1 && consec >= 3)
        {
            minSoFar = newTotal;
            System.Console.WriteLine($"foundit:{minSoFar}");
            return newTotal;
        }

        (int straightA, int straightB) = nextSquare(i, j, direction);
        int straight = Recurse2(board, newTotal, straightA, straightB, direction, consec + 1);

        (int leftA, int leftB) = nextSquare(i, j, (direction + 3) % 4);
        int left = consec <= 3 ? int.MaxValue : Recurse2(board, newTotal, leftA, leftB, (direction + 3) % 4, 1);

        (int rightA, int rightB) = nextSquare(i, j, (direction + 1) % 4);
        int right = consec <= 3 ? int.MaxValue : Recurse2(board, newTotal, rightA, rightB, (direction + 1) % 4, 1);

        int min = Math.Min(straight, left);
        return Math.Min(min, right);
    }
    public override ValueTask<string> Solve_1()
    { 
        return new($"0");
    }

    public override ValueTask<string> Solve_2()
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

        int left = Recurse2(board, -board[0][0], 0, 0, 0, 1);
        int down = Recurse2(board, -board[0][0], 0, 0, 1, 1);
        int total = Math.Min(left, down);
        return new($"{total}");
    }
}