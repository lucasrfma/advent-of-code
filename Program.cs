using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Aoc;

internal class Program
{
    private static void Main(string[] args)
    {
        var infoLogger = LoggerFactory.Create(builder => builder.AddConsole(c =>
            {
                c.TimestampFormat = "[HH:mm:ss] ";
            })
            .SetMinimumLevel(LogLevel.Information)).CreateLogger<Program>();
        var errorLogger = LoggerFactory.Create(builder => builder.AddConsole(c =>
            {
                c.TimestampFormat = "[HH:mm:ss] ";
            })
            .SetMinimumLevel(LogLevel.Error)).CreateLogger<Program>();
         
        if (args.Length < 3)
        {
            infoLogger.LogError("Not enough parameters. Expected: day (one, two ...) " +
                              "+ 1|2 (part one or two) " +
                              "+ 1|2 (ex-input.txt or input.txt) " +
                              "+ (optional) repeater (int - run the solution multiple times to test performance)");
            return;
        }

        string programAndPart = args[0] + args[1];
        string input = args[2] switch
        {
            "1" => args[0] + "\\ex-input.txt",
            "2" => args[0] + "\\input.txt",
        };
        
        if (args.Length == 3)
        {
            run(programAndPart, input, infoLogger);
            return;
        }
        int repeat = int.Parse(args[3]);
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < repeat; i++)
        {
            run(programAndPart, input, errorLogger);
        }
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
        infoLogger.LogInformation("RunTime " + elapsedTime);
    }

    private static void run(string programAndPart, string input, ILogger logger)
    {
        StreamReader sr;
        try
        {
            sr = new(input);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not read file {input}");
            Console.WriteLine(e.Message);
            return;
        }

        switch (programAndPart)
        {
            case "one1":
                Day1.PartOne(sr,logger);
                break;
            case "one2":
                Day1.PartTwo(sr,logger);
                break;
            case "two1":
                Day2.PartOne(sr,logger);
                break;
            case "two2":
                Day2.PartTwo(sr,logger);
                break;
            case "three1":
                Day3.PartOne(sr,logger);
                break;
            case "three2":
                Day3.PartTwo(sr,logger);
                break;
            case "four1":
                Day4.PartOne(sr,logger);
                break;
            case "four2":
                Day4.PartTwo(sr,logger);
                break;
            case "five1":
                Day5.PartOne(sr,logger);
                break;
            case "five2":
                Day5.PartTwo(sr,logger);
                break;
            case "six1":
                new Day6(sr, logger).PartOne();
                break;
            case "six2":
                new Day6(sr, logger).PartTwo();
                break;
        }
    }
}