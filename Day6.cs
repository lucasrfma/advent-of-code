using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day6(StreamReader sr, ILogger logger)
{
    internal struct TimeAndRecord
    {
        internal long Time { get; set; }
        internal long Record { get; set; }
    }

    internal long NumberOfWays(TimeAndRecord tr)
    {
        long a = -1;
        long b = tr.Time;
        long c = -tr.Record;
        double sqrtD = Math.Sqrt(b * b - 4 * a * c);
        double max = (long)(-b - sqrtD) / 2 * a;
        double min = (long)(-b + sqrtD) / 2 * a;
        long lmax = (long)max;
        long lmin = (long)min;
        if (lmax < max) lmax--;

        logger.LogInformation($"a: {a} b: {b} c: {c} sqrtD: {sqrtD} min: {min} max: {max}");
        return lmax - lmin;
    }

    public void PartOne()
    {
        logger.LogInformation("Part One");

        List<long> times = sr.ReadLine()!.Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToList();

        List<long> records = sr.ReadLine()!.Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToList();

        List<TimeAndRecord> timeAndRecords = times.Select((s, i) => new TimeAndRecord
        {
            Time = s,
            Record = records[i]
        }).ToList();

        long product = timeAndRecords.Aggregate(1l, (p, tr) => p * NumberOfWays(tr));

        logger.LogInformation("Product: " + product);
    }

    public void PartTwo()
    {
        logger.LogInformation("Part Two");

        long time = long.Parse(sr.ReadLine()!.Split(':')[1]
            .Split(' ', StringSplitOptions.TrimEntries).Aggregate("", (r, s) => r + s));
        long record = long.Parse(sr.ReadLine()!.Split(':')[1]
            .Split(' ', StringSplitOptions.TrimEntries).Aggregate("", (r, s) => r + s));

        TimeAndRecord timeAndRecord = new() { Time = time, Record = record };

        logger.LogInformation(NumberOfWays(timeAndRecord) + "");
    }
}