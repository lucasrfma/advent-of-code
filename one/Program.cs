namespace One;

internal class Program 
{
    static readonly Dictionary<string,char> digitMap = new() {
            {"one", '1'},
            {"two", '2'},
            {"three", '3'},
            {"four", '4'},
            {"five", '5'},
            {"six", '6'},
            {"seven", '7'},
            {"eight", '8'},
            {"nine", '9'},
        };
    static void Main(string[] args) 
    {
        string inputFile = "input.txt";
        string solution = "";
        if (args.Length > 0)
        {
            inputFile = args[0];
            if (args.Length > 1)
            {
                solution = args[1];
            }
        }

        StreamReader sr;
        try
        {
            sr = new(inputFile);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not read file {args[0]}");
            Console.WriteLine(e.Message);
            return;
        }

        switch (solution)
        {
            case "1": PartOne(sr); break;
            default: PartTwo(sr); break;
        }
    }

    static void PartTwo(StreamReader sr)
    {
        Console.WriteLine("Part Two");
        int sum = 0;

        while(!sr.EndOfStream) {
            string line = sr.ReadLine()!;

            char firstDigit = FirstDigit(line);
            char lastDigit = LastDigit(line);

            var lineNumber = $"{firstDigit}{lastDigit}";
            sum += int.Parse($"{lineNumber}");

        }
        Console.WriteLine($"Sum: {sum}");
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
            string jumble = toEvaluate[0..i];
            foreach( var (key, value) in digitMap )
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
            foreach( var (key, value) in digitMap )
            {
                if (jumble.Contains(key))
                {
                    return value;
                }
            }
        }
        return default;
    }

    static void PartOne(StreamReader sr)
    {
        Console.WriteLine("Part One");
        int sum = 0;

        while(!sr.EndOfStream) {
            string line = sr.ReadLine()!;
            Console.WriteLine(line);

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
        Console.WriteLine($"Sum: {sum}.");
    }

}
