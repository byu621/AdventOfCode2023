namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly string[] _lines;
    private Dictionary<char,int> cardMap = new()
    {
        {'A',14}, {'K',13}, {'Q',12},{'J',1},{'T',10},{'9',9},{'8',8},{'7',7},{'6',6},{'5',5},{'4',4},{'3',3},{'2',2},
    };

    public Day07()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        Dictionary<string,int> dict = new();
        List<string> hands = new();
        foreach(string line in _lines)
        {
            string hand = line.Split(" ")[0];
            int bid = int.Parse(line.Split(" ")[1]);
            dict[hand] = bid;

            hands.Add(hand);
        }

        hands.Sort((string a, string b) => {
            int typeA = GetType(a);
            int typeB = GetType(b);
            if (typeA != typeB)
            {
                return typeA - typeB;
            }

            for (int i = 0; i < 5; i++)
            {
                if (a[i] != b[i])
                {
                    int valA = cardMap[a[i]];
                    int valB = cardMap[b[i]];
                    return valA - valB;
                }
            }

            return 1;
        });



        long total = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            int bid = dict[hands[i]];
            total += (i+1) * bid; 
        }
        return new($"{total}");
    }

    private int GetType(string hand)
    {
        var charCounts = hand.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
        if (charCounts.Any(x => x.Value == 5))
        {
            return 7;
        }

        if (charCounts.Any(x => x.Value == 4))
        {
            return 6;
        }

        if (charCounts.Any(x => x.Value == 3) && charCounts.Any(x => x.Value == 2))
        {
            return 5;
        }

        if (charCounts.Any(x => x.Value == 3) )
        {
            return 4;
        }


        if (charCounts.Count(kv => kv.Value == 2) == 2)
        {
            return 3;
        }

        if (charCounts.Any(x => x.Value == 2) )
        {
            return 2;
        }
        return 1;
    }
    private int GetType2(string hand)
    {
        var charCounts = hand.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
        int numberOfJ = charCounts.ContainsKey('J') ? charCounts['J']:0;
        if (numberOfJ == 0) return GetType(hand);
        charCounts.Remove('J');
        int maxSame = charCounts.Count() == 0 ? 0 : charCounts.Max(x => x.Value);
        int maxSame2 = charCounts.Count() <= 1 ? 0 : charCounts.OrderByDescending(x => x.Value).Skip(1).First().Value;
        if (maxSame == 5 || maxSame + numberOfJ == 5)
        {
            return 7;
        }

        if (maxSame == 4 || maxSame + numberOfJ == 4)
        {
            return 6;
        }

        // bool fh1 = maxSame == 3 && maxSame2 == 2;
        // bool fh2 = maxSame == 2 && maxSame2 == 2 && numberOfJ == 1;
        bool fh3 = maxSame + maxSame2 + numberOfJ == 5;
        if (fh3)
        {
            return 5;
        }

        if (maxSame + numberOfJ == 3)
        {
            return 4;
        }

        bool tp1 = maxSame == 2 && maxSame2 == 2;
        if (tp1)
        {
            return 3;
        }

        if (numberOfJ > 0 || maxSame == 2)
        {
            return 2;
        }
        return 1;
    }


    public override ValueTask<string> Solve_2()
    {
        Dictionary<string,int> dict = new();
        List<string> hands = new();
        foreach(string line in _lines)
        {
            string hand = line.Split(" ")[0];
            int bid = int.Parse(line.Split(" ")[1]);
            dict[hand] = bid;

            hands.Add(hand);
        }

        hands.Sort((string a, string b) => {
            int typeA = GetType2(a);
            int typeB = GetType2(b);
            if (typeA != typeB)
            {
                return typeA - typeB;
            }

            for (int i = 0; i < 5; i++)
            {
                if (a[i] != b[i])
                {
                    int valA = cardMap[a[i]];
                    int valB = cardMap[b[i]];
                    return valA - valB;
                }
            }

            return 0;
        });

        // foreach(var a in hands) System.Console.WriteLine(a);

        long total = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            int bid = dict[hands[i]];
            total += (i+1) * bid; 
        }

        System.Console.WriteLine(GetType2("A2345"));
        return new($"{total}");
    }
}