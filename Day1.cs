using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day1(StreamReader sr, ILogger logger)
{
    static readonly Dictionary<string,char> DigitMap = new() {
            {"one", '1'},
            {"two", '2'},
            {"three", '3'},
            {"four", '4'},
            {"five", '5'},
            {"six", '6'},
            {"seven", '7'},
            {"eight", '8'},
            {"nine", '9'}
    };

    public void PartTwo()
    {
        logger.LogInformation("Part Two");
        int sum = 0;

        while(!sr.EndOfStream) {
            string line = sr.ReadLine()!;

            char firstDigit = FirstDigit(line);
            char lastDigit = LastDigit(line);

            var lineNumber = $"{firstDigit}{lastDigit}";
            sum += int.Parse($"{lineNumber}");

        }
        logger.LogInformation($"Sum: {sum}");
    }

    static char FirstDigit(string toEvaluate)
    {
        for (int i = 1; i <= toEvaluate.Length; i++)
        {
            char c = toEvaluate[i-1];
                if (c >= '0' && c <= '9') 
                {
                    return c;
                }
            string jumble = toEvaluate[..i];
            foreach( var (key, value) in DigitMap )
            {
                if (jumble.Contains(key))
                {
                    return value;
                }
            }
        }
        return default;
    }
    static char LastDigit(string toEvaluate)
    {
        for (int i = 1; i <= toEvaluate.Length; i++)
        {
            char c = toEvaluate[^i];
            if (c >= '0' && c <= '9') 
            {
                return c;
            }
            string jumble = toEvaluate[^i..];
            foreach( var (key, value) in DigitMap )
            {
                if (jumble.Contains(key))
                {
                    return value;
                }
            }
        }
        return default;
    }

    public void PartOne()
    {
        logger.LogInformation("Part One");
        int sum = 0;

        while(!sr.EndOfStream) {
            string line = sr.ReadLine()!;

            char firstDigit = default;
            char lastDigit = default;

            foreach (char c in line) {
                if (c >= '0' && c <= '9') 
                {
                    if (firstDigit == default) 
                    {
                        firstDigit = c;
                    }
                    lastDigit = c;
                }

            }

            if (lastDigit != default)
            {
                sum += int.Parse($"{firstDigit}{lastDigit}");
            }
        }
        logger.LogInformation($"Sum: {sum}.");
    }

}
