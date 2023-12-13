
namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly string[] _lines;

    public Day10()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    private int Recurse(int a, int b, int a2, int b2, int count, List<(int, int)> seen)
    {
        if (a == a2 && b == b2)
        {
            seen.Add((a,b));
            return count;
        }

        seen.Add((a,b));
        seen.Add((a2,b2));

        (int na, int nb) = TakePipe(a,b,seen);
        (int na2, int nb2) = TakePipe(a2,b2,seen);

        return Recurse(na, nb, na2, nb2, count+1, seen);
    }

    private (int, int) TakePipe(int a, int b, List<(int, int)> seen)
    {
        char pipe = _lines[a][b];
        if (pipe == '|')
        {
            if (!seen.Contains((a-1,b))) return (a-1,b);
            if (!seen.Contains((a+1,b))) return (a+1,b);
        } else if (pipe == '-') 
        {
            if (!seen.Contains((a,b-1))) return (a,b-1);
            if (!seen.Contains((a,b+1))) return (a,b+1);
        } else if (pipe == 'L')
        {
            if (!seen.Contains((a-1,b))) return (a-1,b);
            if (!seen.Contains((a,b+1))) return (a,b+1);
        } else if (pipe == 'J')
        {
            if (!seen.Contains((a-1,b))) return (a-1,b);
            if (!seen.Contains((a,b-1))) return (a,b-1);
        } else if (pipe == '7')
        {
            if (!seen.Contains((a+1,b))) return (a+1,b);
            if (!seen.Contains((a,b-1))) return (a,b-1);
        } else if (pipe == 'F')
        {
            if (!seen.Contains((a+1,b))) return (a+1,b);
            if (!seen.Contains((a,b+1))) return (a,b+1);
        } 

        throw new Exception();
    }

    public override ValueTask<string> Solve_1()
    {
        for (int i = 0; i < _lines.Length; i++)
        {
            for (int j = 0; j < _lines[i].Length; j++)
            {
                if (_lines[i][j] == 'S')
                {
                    char sPipe = 'F';
                    List<(int,int)> seen = new();
                    seen.Add((i,j));
                    int output = Recurse(i + 1, j, i, j + 1, 1, seen);
                    return new($"{output}");
                }
            }
        }
        throw new NotImplementedException();
    }

    public override ValueTask<string> Solve_2()
    {
        List<(int,int)> seen = new();
        for (int i = 0; i < _lines.Length; i++)
        {
            for (int j = 0; j < _lines[i].Length; j++)
            {
                if (_lines[i][j] == 'S')
                {
                    seen.Add((i,j));
                    int output = Recurse(i + 1, j, i, j + 1, 1, seen);
                    break;
                }
            }
        }

        int insideCount = 0;
        HashSet<(int,int,bool,bool)> visited = new();
        for (int i = 0 ; i < _lines.Length; i++)
        {
            for (int j = 0; j < _lines[i].Length; j++)
            {
                if (seen.Contains((i,j))) continue;
                if (visited.Contains((i, j, false, false))) continue;
                visited.Add((i, j , false,false));
                if (!IsOutside(visited, seen)) insideCount++;
            }
        }
        return new($"{insideCount}");
    }

    private bool IsOutside(HashSet<(int,int,bool,bool)> visited, List<(int,int)> seen)
    {
        foreach(var square in visited)
        {
            if (square.Item1 == 0 || square.Item1 == _lines.Length - 1 || square.Item2 == 0 || square.Item2 == _lines[0].Length - 1)
            {
                return true;
            }
        }

        // up down left right upleft.5 upright.5 downleft.5 downright.5
        // right up
        foreach(var square in visited)
        {
            if (!square.Item3 && !square.Item4)
            {
                int upI = square.Item1 - 1;
                int upJ = square.Item2;
                if (!seen.Contains((upI, upJ)) && !visited.Contains((upI, upJ, false, false)))
                {
                    visited.Add((upI, upJ, false, false));
                    return IsOutside(visited, seen);
                }

                int leftI = square.Item1;
                int leftJ = square.Item2 - 1;
                if (!seen.Contains((leftI, leftJ)) && !visited.Contains((leftI, leftJ, false, false)))
                {
                    visited.Add((leftI, leftJ, false, false));
                    return IsOutside(visited, seen);
                }

                int rightI = square.Item1;
                int rightJ = square.Item2 + 1;
                if (!seen.Contains((rightI, rightJ)) && !visited.Contains((rightI, rightJ, false, false)))
                {
                    visited.Add((rightI, rightJ, false, false));
                    return IsOutside(visited, seen);
                }

                int downI = square.Item1 + 1;
                int downJ = square.Item2;
                if (!seen.Contains((downI, downJ)) && !visited.Contains((downI, downJ, false, false)))
                {
                    visited.Add((downI, downJ, false, false));
                    return IsOutside(visited, seen);
                }

                char upChar = _lines[upI][upJ];
                if (seen.Contains((upI, upJ)) && (upChar == 'L' || upChar == 'F' || upChar == '|'))
                {
                    int upLeftI = square.Item1 - 1;
                    int upLeftJ = square.Item2 - 1;
                    if (!visited.Contains((upLeftI, upLeftJ, true, false)))
                    {
                        visited.Add((upLeftI, upLeftJ, true, false));
                        return IsOutside(visited, seen);
                    }
                }               

                if (seen.Contains((upI, upJ)) && (upChar == 'J' || upChar == '7' || upChar == '|'))
                {
                    int upRightI = square.Item1 - 1;
                    int upRightJ = square.Item2;
                    if (!visited.Contains((upRightI, upRightJ, true, false)))
                    {
                        visited.Add((upRightI, upRightJ, true, false));
                        return IsOutside(visited, seen);
                    }
                }
            }

            if (square.Item3)
            {
                // up
                int upLeftI = square.Item1 - 1;
                int upLeftJ = square.Item2;
                int upRightI = square.Item1 - 1;
                int upRightJ = square.Item2 + 1;
                if (seen.Contains((upLeftI, upLeftJ)) && seen.Contains((upRightI, upRightJ)))
                {
                    char upLeftChar = _lines[upLeftI][upLeftJ];
                    char upRightChar = _lines[upRightI][upRightJ];
                    if (upLeftChar != 'L' && upLeftChar != 'F' && upLeftChar != '-' && upRightChar != '7' && upRightChar != 'J' && upRightChar != '-')
                    {
                        if (!visited.Contains((upLeftI, upLeftJ, true, false)))
                        {
                            visited.Add((upLeftI, upLeftJ, true, false));
                            return IsOutside(visited, seen);
                        }
                    }
                }
                
                // left
                int leftI = square.Item1;
                int leftJ = square.Item2;
                char leftChar = _lines[leftI][leftJ];
                if (leftChar == '7')
                {
                    if (seen.Contains((leftI, leftJ)) && seen.Contains((upLeftI, upLeftJ)))
                    {
                        if (!visited.Contains((leftI, leftJ, false, true)))
                        {
                            visited.Add((leftI, leftJ, false, true));
                            return IsOutside(visited, seen);
                        }
                    }
                }
            }

            if (square.Item4)
            {
                // left
                int leftDownI = square.Item1;
                int leftDownJ = square.Item2 - 1;
                int leftUpI = square.Item1 - 1;
                int leftUpJ = square.Item2 - 1;
                if (seen.Contains((leftDownI, leftDownJ)) && seen.Contains((leftUpI, leftUpJ)))
                {
                    char leftDownChar = _lines[leftDownI][leftDownJ];
                    char leftUpChar = _lines[leftUpI][leftUpJ];
                    if (leftDownChar != 'L' && leftDownChar != 'J' && leftDownChar != '|' && leftUpChar != '7' && leftUpChar != 'F' && leftUpChar != '|')
                    {
                        if (!visited.Contains((leftDownI, leftDownJ, false, true)))
                        {
                            System.Console.WriteLine($"{leftDownI},{leftDownJ}");
                            visited.Add((leftDownI, leftDownJ, false, true));
                            return IsOutside(visited, seen);
                        }
                    }
                }
 
            }
        }

        return false;
    }
}