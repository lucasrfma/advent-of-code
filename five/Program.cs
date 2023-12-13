using Microsoft.Extensions.Logging;

namespace Five;

public class Program
{
    internal class Converter()
    {
        List<ConverterItem> ConverterItems { get; init; } = [];

        internal long Convert(long baseNumber)
        {
            var ci = ConverterItems.Find(ci =>
                ci.BaseStart <= baseNumber && baseNumber < ci.BaseStart + ci.RangeLength
            );
            return ci is null ? baseNumber : baseNumber - ci.BaseStart + ci.TargetStart;
        }

        internal List<AttributeInfo> Convert(AttributeInfo attributeInfo)
        {
            List<AttributeInfo> result = [];
            long leftToAdd = attributeInfo.Length;
            long siStart = attributeInfo.Start;
            for (int i = 0; i < ConverterItems.Count && leftToAdd > 0; i++)
            {
                ConverterItem ci = ConverterItems[i];
                if (ci.BaseStart <= siStart && siStart < ci.BaseStart + ci.RangeLength)
                {
                    long startDelta = siStart - ci.BaseStart;
                    long ciRangeLeft = ci.RangeLength - startDelta;
                    long adding = ciRangeLeft > leftToAdd ? leftToAdd : ciRangeLeft;
                    leftToAdd -= adding;
                    siStart += adding;
                    result.Add(new (ci.TargetStart+startDelta,adding));
                }
            }
            if (leftToAdd > 0)
            {
                long restStart = attributeInfo.Start + (attributeInfo.Length - leftToAdd);
                result.Add(new (restStart,leftToAdd));
            }
            return result;
        }

        internal Converter(StreamReader sr) : this()
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine()!;
                if (line == "") break;
                long[] ci = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse).ToArray();
                ConverterItems.Add(new(ci[0], ci[1], ci[2]));
            }

            ConverterItems = ConverterItems.OrderBy(ci => ci.BaseStart).ToList();
        }
    }

    record ConverterItem(long TargetStart, long BaseStart, long RangeLength);

    public static void PartOne(StreamReader sr, ILogger logger)
    {
        logger.LogInformation("Part One");
        long[] seeds = sr.ReadLine()!.Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToArray();
        sr.ReadLine();

        List<Converter> converterList = [];
        // there would be more generic ways to do this, but I decided that since the input file
        // has a very well define structure, it would be interested to program specifically for said
        // structure
        for (int i = 0; i < 7; ++i)
        {
            sr.ReadLine();
            converterList.Add(new Converter(sr));
        }

        long closest = seeds.Select(s =>
            converterList.Aggregate(s, (seed, converter) => converter.Convert(seed))
        ).Min();

        logger.LogInformation("Closest: " + closest);
    }
    
    internal static long ClosestLocationSeedRange2(AttributeInfo seedInfo, List<Converter> converterList)
    {
        List<AttributeInfo> attributeInfos = [seedInfo];
        foreach (var cvt in converterList)
        {
            attributeInfos = attributeInfos.SelectMany(ai => cvt.Convert(ai), (_, nai) => nai).ToList();
        }
        return attributeInfos.Select(ai => ai.Start).Min();
    }

    internal record struct AttributeInfo(long Start, long Length);

    public static void PartTwo(StreamReader sr, ILogger logger)
    {
        logger.LogInformation("Part Two");
        string[] roughSeedInfo = sr.ReadLine()!.Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        List<AttributeInfo> seedInfos = [];
        for (int i = 0; i < roughSeedInfo.Length; i += 2)
        {
            AttributeInfo seedInfo = new(long.Parse(roughSeedInfo[i]), long.Parse(roughSeedInfo[i + 1]));
            seedInfos.Add(seedInfo);
            logger.LogInformation($"Seed info: {seedInfo}");
        }

        sr.ReadLine();

        List<Converter> converterList = [];
        // there would be more generic ways to do this, but I decided that since the input file
        // has a very well define structure, it would be interested to program specifically for said
        // structure
        for (int i = 0; i < 7; ++i)
        {
            sr.ReadLine();
            converterList.Add(new Converter(sr));
        }

        long closest = seedInfos
            .Select(si =>
            {
                logger.LogInformation($"Processing seed info: {si}");
                return ClosestLocationSeedRange2(si, converterList);
            }).Min();

        logger.LogInformation("Closest: " + closest);
    }
}