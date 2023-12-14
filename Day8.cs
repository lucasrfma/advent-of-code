using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day8(StreamReader sr, ILogger logger)
{
    
    public void PartOne()
    {
        logger.LogInformation("Part One");
        string destination = "ZZZ";
        string instructions = sr.ReadLine()!;
        sr.ReadLine();

        var map = ReadMap();

        string currentPosition = "AAA";
        long stepsTaken = 0;
        
        int i = 0;
        while (currentPosition != destination)
        {
            var nextCandidate = map[currentPosition];
            currentPosition = instructions[i] == 'L' ? nextCandidate.l : nextCandidate.r;
            i = ++i == instructions.Length ? 0 : i;
            stepsTaken++;
        }

        Console.WriteLine($"Steps taken: {stepsTaken}");
    }
    
    public void PartTwo()
    {
        logger.LogInformation("Part Two");
        var instructions = sr.ReadLine()!;
        sr.ReadLine();

        var (map, currentPositions) = ReadMapAndStartingPoints();
        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        List<long> cycleLengths = currentPositions.Select(cp =>
        {
            var i = 0;
            long cycleLength = 0L;
            while (cp[2] != 'Z')
            {
                var nextCandidates = map[cp];
                cp = instructions[i] == 'L' ? nextCandidates.l : nextCandidates.r;
                i = ++i == instructions.Length ? 0 : i;
                cycleLength++;
            }

            return cycleLength;
        }).ToList();

        long stepsTaken = cycleLengths.Aggregate(Lvm);

        logger.LogInformation($"Steps taken: {stepsTaken}");
    }

    private Dictionary<string, (string l, string r)> ReadMap()
    {
        Dictionary<string, (string, string)> map = new Dictionary<string, (string, string)>();
        while (!sr.EndOfStream)
        {
            string[] line = sr.ReadLine()!.Split('=',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            string key = line[0];
            string removingParans = line[1].Substring(1, line[1].Length - 2);

            string[] lr = removingParans.Split(',', StringSplitOptions.TrimEntries);
            map.Add(key,(lr[0],lr[1]));
        }

        return map;
    }

    private (Dictionary<string, (string l, string r)>, List<string>) ReadMapAndStartingPoints()
    {
        Dictionary<string, (string, string)> map = new Dictionary<string, (string, string)>();
        List<string> startingPoints = [];
        
        while (!sr.EndOfStream)
        {
            string[] line = sr.ReadLine()!.Split('=',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            string key = line[0];
            string removingParans = line[1].Substring(1, line[1].Length - 2);

            string[] lr = removingParans.Split(',', StringSplitOptions.TrimEntries);
            map.Add(key,(lr[0],lr[1]));
            
            if (key[2] == 'A') startingPoints.Add(key);
        }

        return (map,startingPoints);
    }

    private long Lvm(long a, long b)
    {
        var (bigger, smaller) = a > b ? (a,b) : (b,a);
        long mod = bigger % smaller;
        while (mod != 0)
        {
            bigger = smaller;
            smaller = mod;
            mod = bigger % smaller;
        }

        return a * b / smaller;
    }

}