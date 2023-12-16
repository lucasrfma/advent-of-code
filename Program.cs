using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Aoc;

internal class Program
{
    private static void Main(string[] args)
    {
        var infoLogger = LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                })
            .SetMinimumLevel(LogLevel.Information)).CreateLogger<Program>();
        var errorLogger = LoggerFactory.Create(builder =>
            builder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.SingleLine = true;
                options.TimestampFormat = "HH:mm:ss ";
            })
            .SetMinimumLevel(LogLevel.Error)).CreateLogger<Program>();
         
        if (args.Length < 3)
        {
            infoLogger.LogError("""
            Not enough parameters. Expected (in order):
            d# :                d1, d2...
            p1|p2 :             part one or two.
            ex|fn|file-name :   ex and fn are shortcuts for ex-input.txt and input.txt.
                                Otherwise specify the whole name.
                                Will look for file in path: input-files/d#/file-name.
            # (Optional) :      How many times the program should run.
                                When running with this option, the program will not log results,
                                instead it will log how long it took to run.
            """);
            return;
        }

        string input = args[2] switch
        {
            "ex" => $@"input-files\{args[0]}\ex-input.txt",
            "fn" => $@"input-files\{args[0]}\input.txt",
            { } any => $@"input-files\{args[0]}\{any}",
        };

        string className = $"Day{args[0][1..]}";
        string methodName = args[1] switch
        {
            "p1" => "PartOne",
            "p2" => "PartTwo",
            _ => throw new ArgumentException("Second parameter should be either p1 or p2")
        };


        if (args.Length == 3)
        {
            var (instance, method) = GetInstanceAndMethod(className,methodName,input,infoLogger);
            method.Invoke(instance,null);
            return;
        }
        {
            int repeat = int.Parse(args[3]);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < repeat; i++)
            {
                var (instance, method) = GetInstanceAndMethod(className,methodName,input,errorLogger);
                method.Invoke(instance,null);
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            infoLogger.LogInformation("RunTime " + elapsedTime);
            Console.WriteLine("RunTime " + elapsedTime);
        }
    }

    private static (object,MethodInfo) GetInstanceAndMethod(string className, string methodName, string input, ILogger logger)
    {
        try
        {
            StreamReader sr = new(input);
            Type? type = Type.GetType($"Aoc.{className}");
            if (type == null)
            {
                logger.LogError($"Could not get type {className}");
                Environment.Exit(1);
            }
            object[] parameters = {sr,logger};
            object? instance = Activator.CreateInstance(type, parameters);
            if (instance == null)
            {
                logger.LogError($"Could not instantiate class {className} with parameters StreamReader and ILogger");
                Environment.Exit(1);
            }
            MethodInfo? method = type.GetMethod(methodName);
            if (method == null)
            {
                logger.LogError($"Could not get method {methodName} from class {className}");
                Environment.Exit(1);
            }
            return (instance,method);
        }
        catch (Exception e)
        {
            logger.LogError($"Could not read file {input}");
            logger.LogError(e.Message);
            Environment.Exit(1);
        }
        return (null,null);
    }

}