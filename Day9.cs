using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day9(StreamReader sr, ILogger logger)
{
    public void PartOne()
    {
        logger.LogInformation("Part One and Two");

        long future = 0;
        long past = 0;
        while (!sr.EndOfStream)
        {
            var readings = sr.ReadLine()!.Split(' ').Select(long.Parse).ToList();
            List<List<long>> subtractions = [readings];
            
            (bool ok, int depth) res = (false, default);
            while (!res.ok)
            {
                res = Probe(subtractions,readings.Count);
            }

            var depth = res.depth;
            
            subtractions[^2].Add(subtractions[^2][0]);
            for (int i = subtractions.Count - 3; i >= 0; i--)
            {
                subtractions[i].Add(subtractions[i][^1] + subtractions[i + 1][^1]);
            }
            future += subtractions[0][^1];
            
            for (int i = depth - 2; i >= 0; i--)
            {
                subtractions[i][0] -= subtractions[i + 1][0];
            }
            past += subtractions[0][0];
            
            (bool ok, int depth) Probe(List<List<long>> _subtractions, int _buffer)
            {
                int max = _buffer;
                for (int i = 0; i < max - 1; i++)
                {
                    if (_subtractions.Count == i + 1)
                        _subtractions.Add([]);

                    var range = _subtractions[i][..(max - i)];
                    _subtractions[i + 1].AddRange(SelectSubtractions(range, _subtractions[i + 1]));
                    
                    if (subtractions[i + 1].All(l => l == 0))
                        return (true, i + 2);
                }
                return (false,default);
            }
        }
        logger.LogInformation("Future: "+future);
        logger.LogInformation("Past: "+past);
        
        IEnumerable<long> SelectSubtractions(IReadOnlyList<long> origin,
            IReadOnlyList<long> output)
        {
            for (int i = output.Count; i < origin.Count - 1; i++)
            {
                long res = origin[i + 1] - origin[i];
                yield return res;
            }
        }
    }

    public void PartTwo()
    {
        logger.LogInformation("Part Two is together with p1 this time.");
    }
}