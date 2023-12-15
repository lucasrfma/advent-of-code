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
            // **Commented lines are for buffer version**
            // int bufferUnit = 7;
            // int bufferUnit = 11;
            // int bufferUnit = 21;
            // int buffer = 0;
            
            (bool ok, int depth) res = (false, default);
            while (!res.ok)
            {
                // buffer += bufferUnit;
                // res = Probe(subtractions,buffer);
                res = Probe(subtractions,readings.Count);
            }

            var depth = res.depth;

            // var ending = readings[^depth..];
            // List<List<long>> resCalc = [ending];
            // Probe(resCalc,ending.Count);
            // resCalc[^2].Add(resCalc[^2][0]);

            // for (int i = resCalc.Count - 3; i >= 0; i--)
            // {
            //     resCalc[i].Add(resCalc[i][^1] + resCalc[i + 1][^1]);
            // }
            // future += resCalc[0][^1];
            
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
                // int max = _buffer > _subtractions[0].Count ? _subtractions[0].Count : _buffer;
                int max = _buffer;
                for (int i = 0; i < max - 1; i++)
                {
                    if (_subtractions.Count == i + 1)
                        _subtractions.Add([]);

                    var range = _subtractions[i][..(max - i)];
                    _subtractions[i + 1].AddRange(SelectSubtractions(range, _subtractions[i + 1]));

                    var _depth = i + 1;

                    // if (max == _subtractions[0].Count || _subtractions[i + 1].Count >= 4)
                    // {
                        if (subtractions[i + 1].All(l => l == 0))
                            return (true, _depth + 1);
                    // }
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

// Version                                                          runs   : test 1 : test 2 : test 3
// buffer set to 7  (1/3full)                                       1.000x : 1.963s : 1.791s : 1.921s
// buffer set to 11 (1/2full)                                       1.000x : 1.738s : 1,776s : 1.925s
// buffer equal to full                                             1.000x : 1,856s : 1,796s : 1,749s
// full (less checks)                                               1.000x : 1.723s : 1.814s : 1.663s
// full (less checks, using subtractions matrix for future calc)    1.000x : 1.463s : 1.468s : 1.572s

    public void PartTwo()
    {
        logger.LogInformation("Part Two is together with p1 this time.");
    }
}