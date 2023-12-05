
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;

namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string[] _lines;

    public Day05()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<long> seeds = new();
        List<List<(long, long, long)>> mappings = new();

        seeds = _lines[0].Split(": ")[1].Split(" ").Select(long.Parse).ToList();

        long mapIndex = -1;

        for (long i = 2; i < _lines.Length; i++)
        {
            string line = _lines[i];
            if (line == "") continue;
            if (line.Contains(":"))
            {
                mapIndex++;
                mappings.Add(new());
                continue;
            }

            List<long> split = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            mappings[(int)mapIndex].Add((split[0], split[1], split[2]));
        }

        long min = long.MaxValue;
        foreach(long seed in seeds)
        {
            min = Math.Min(min, Recurse(seed, 0, mappings));
        }

        return new($"{min}");
    }

    private long Recurse(long current, long mapIndex, List<List<(long, long, long)>> mappings)
    {
        if (mapIndex == 7)
        {
            return current;
        }

        List<(long,long,long)> list = mappings[(int)mapIndex];
        foreach((long,long,long) map in list)
        {
            if (map.Item2 <= current && map.Item2 + map.Item3 - 1 >= current)
            {
                return Recurse(map.Item1 - map.Item2 + current, mapIndex + 1, mappings);
            }
        }

        return Recurse(current, mapIndex + 1, mappings);
    }

    // private long Recurse2(long current, long range, long mapIndex, List<List<(long, long, long)>> mappings)
    // {
    //     if (mapIndex == 7)
    //     {
    //         return current;
    //     }

    //     List<(long,long,long)> list = mappings[(int)mapIndex];
    //     foreach((long,long,long) map in list)
    //     {
    //         if (map.Item2 <= current && map.Item2 + map.Item3 - 1 >= current)
    //         {
    //             return Recurse2(map.Item1 - map.Item2 + current, mapIndex + 1, mappings);
    //         }
    //     }

    //     return Recurse2(current, mapIndex + 1, mappings);
    // }
    private (List<long>, List<List<(long, long, long)>>) ExtractInitial()
    {
         List<long> seeds = new();
        List<List<(long, long, long)>> mappings = new();

        seeds = _lines[0].Split(": ")[1].Split(" ").Select(long.Parse).ToList();

        long mapIndex = -1;

        for (long i = 2; i < _lines.Length; i++)
        {
            string line = _lines[i];
            if (line == "") continue;
            if (line.Contains(":"))
            {
                mapIndex++;
                mappings.Add(new());
                continue;
            }

            List<long> split = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            mappings[(int)mapIndex].Add((split[0], split[1], split[2]));
        }

        return (seeds, mappings);
    }

    private List<(long,long)> GivenMappingGiveDestinationGroupings(List<(long,long,long)> mapping)
    {
        List<(long, long)> output = new();
        var orderedMapping = mapping.OrderBy(m=>m.Item1).ToList();

        if (orderedMapping[0].Item1 != 0)
        {
            output.Add((0, orderedMapping[0].Item1-1));
        }
        foreach(var m in orderedMapping)
        {
            output.Add((m.Item1, m.Item1 + m.Item3 - 1));
        }
        output.Add((output.Last().Item2 + 1, long.MaxValue));

        return output;
    }

    private List<(long,long)> GetSourceGroupingsGivenTrivial(List<(long,long)> destinationGroupings, List<(long,long,long)> mapping)
    {
        List<(long, long)> output = new();
        var orderedMapping = mapping.OrderBy(m=>m.Item1).ToList();
        foreach(var d in destinationGroupings)
        {
            if (mapping.Count(m => m.Item1 == d.Item1) == 0)
            {
                output.Add(d);
            } else 
            {
                var m = mapping.Where(m=>m.Item1 == d.Item1).First();
                output.Add((m.Item2, m.Item2 + m.Item3 - 1));
            }
        }
        return output;
    }

    private List<(long,long)> GetNewDestinationGroupings(List<(long,long)> destinationGroupings, List<(long,long,long)> mapping)
    {
        List<(long,long)> newDestinationGroupings = new();
        foreach(var dg in destinationGroupings)
        {
            var start = dg.Item1;
            var finish = dg.Item2;
            var orderedMapping = mapping.OrderBy(m=>m.Item1).ToList();
            foreach(var m in orderedMapping)
            {
                if (m.Item1 > start && m.Item1 <= finish)
                {
                    newDestinationGroupings.Add((start, m.Item1 - 1));
                    start = m.Item1;
                }

                var upper = m.Item1 + m.Item3 - 1;
                if (upper >= start && upper < finish)
                {
                    newDestinationGroupings.Add((start, upper));
                    start = upper+1;
                }
            }
            newDestinationGroupings.Add((start,finish));
        }

        return newDestinationGroupings;
    }

    private List<(long,long)> NewDestinationToSource(List<(long,long)> destinationGroupings, List<(long,long,long)> mapping)
    {
        List<(long,long)> source = new();

        foreach(var dg in destinationGroupings)
        {
            bool foundMatching = false;
            foreach(var m in mapping)
            {
                var ma = m.Item1;
                var mb = m.Item1 + m.Item3 - 1;
                if (dg.Item1 >= ma && dg.Item1 <= mb && dg.Item2 >= ma && dg.Item2 <= mb)
                {
                    source.Add((m.Item2 - m.Item1 + dg.Item1, m.Item2 - m.Item1 + dg.Item2));
                    foundMatching = true;
                    break;
                } else {
                    continue;
                }
            }
            if (!foundMatching)
            {
                source.Add((dg.Item1, dg.Item2));
            }
        }

        return source;
    }

    public override ValueTask<string> Solve_2()
    {
        (var seeds, var mappings) = ExtractInitial();

        var locationDestinationGroupings = GivenMappingGiveDestinationGroupings(mappings[6]);
        foreach(var a in locationDestinationGroupings) System.Console.WriteLine(a);
        var humidityDestinationGroupings = GetSourceGroupingsGivenTrivial(locationDestinationGroupings, mappings[6]);
        foreach(var a in humidityDestinationGroupings) System.Console.WriteLine(a);
        var newHumidityDestinationGroupings = GetNewDestinationGroupings(humidityDestinationGroupings, mappings[5]);
        foreach(var a in newHumidityDestinationGroupings) System.Console.WriteLine(a);
        var temperatureDestinationGroupings = NewDestinationToSource(newHumidityDestinationGroupings, mappings[5]);
        foreach(var a in temperatureDestinationGroupings) System.Console.WriteLine(a);
        System.Console.WriteLine();
        var newTemperatureDestinationGroupings = GetNewDestinationGroupings(temperatureDestinationGroupings, mappings[4]);
        foreach(var a in newTemperatureDestinationGroupings) System.Console.WriteLine(a);
        System.Console.WriteLine();

        var lightDestinationGroupings = NewDestinationToSource(newTemperatureDestinationGroupings, mappings[4]);
        foreach(var a in lightDestinationGroupings) System.Console.WriteLine(a);
        System.Console.WriteLine();
        var newLightDGs = GetNewDestinationGroupings(lightDestinationGroupings, mappings[3]);
        var waterDGs = NewDestinationToSource(newLightDGs, mappings[3]);

        var newWaterDGs = GetNewDestinationGroupings(waterDGs, mappings[2]);
        var fertilizerDGs = NewDestinationToSource(newWaterDGs, mappings[2]);

        var newFertilizerDGs = GetNewDestinationGroupings(fertilizerDGs, mappings[1]);
        var soilDGs = NewDestinationToSource(newFertilizerDGs, mappings[1]);

        var newSoilDgs = GetNewDestinationGroupings(soilDGs, mappings[0]);
        var seedDGs = NewDestinationToSource(newSoilDgs, mappings[0]);
        foreach(var a in seedDGs) System.Console.WriteLine(a);

        int minIndex = seedDGs.Count;
        for (int i = 0 ; i < seeds.Count; i+=2)               
        {
            long seed = seeds[i];
            long range = seeds[i+1];

            for (int j = 0; j < seedDGs.Count; j++)
            {
                var sdg = seedDGs[j];
                if (sdg.Item1 >= seed && sdg.Item2 <= seed + range - 1)
                {
                    minIndex = Math.Min(minIndex, j);
                }
            }
        }
        System.Console.WriteLine(seedDGs[minIndex].Item1);
        long seedWinner = seedDGs[minIndex].Item1;

        long winner = Recurse(seedWinner, 0, mappings);

        return new($"{winner}");      
    }
}
