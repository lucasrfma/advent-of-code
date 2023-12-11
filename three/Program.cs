internal class Program
{
    private static void Main()
    {
        // I decided to not read console args anymore.
        string inputFile = "input.txt";
        // string inputFile = "ex-input.txt";
        StreamReader sr;
        try
        {
            sr = new(inputFile);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not read file {inputFile}");
            Console.WriteLine(e.Message);
            return;
        }

        // PartOne(sr);
        PartTwo(sr);
    }

    // I'm not sure if only the symbols present in the example are valid symbols, so we are going for
    // an exclusion logic
    static bool IsSymbol(char c)
    {
        return !char.IsLetterOrDigit(c) && !char.IsControl(c) && c != '.';
    }

    static bool IsValidNp(NumberPosition np, List<string> schematic)
    {
        int startLine = np.LineIndex == 0 ? 0 : np.LineIndex - 1;
        int endLine = np.LineIndex + 1;
        endLine = endLine == schematic.Count ? endLine : endLine + 1;
        for (int i = startLine; i < endLine; i++)
        {
            if (ThereIsSymbol(schematic[i],np.StartIndex,np.EndIndex))
                return true;
        }
        return false;
    }
    static bool ThereIsSymbol(string line, int start, int end)
    {
        start = start == 0 ? 0 : start - 1;
        end = end == line.Length ? end : end + 1;
        for(int i = start; i < end; ++i )
        {
            if (IsSymbol(line[i]))
                return true;
        }
        return false;
    }

    struct NumberPosition()
    {
        internal int Number;
        internal int LineIndex;
        internal int StartIndex;
        internal int EndIndex;

        internal NumberPosition(string line, ref int startIndex, int lineIndex) : this()
        {
            StartIndex = startIndex;
            LineIndex = lineIndex;
            EndIndex = startIndex + 1;

            for (; EndIndex < line.Length; ++EndIndex)
            {
                if (!char.IsDigit(line[EndIndex]))
                    break;
            }
            string n = line[startIndex..EndIndex];
            Number = int.Parse(n);
            startIndex = EndIndex-1;
        }
    }

    record PossibleGear(int Line, int Index);

    static void PartOne(StreamReader sr)
    {
        Console.WriteLine("Part One");
        List<string> schematic = [];

        List<NumberPosition> nps = [];

        for (int lineIndex = 0; !sr.EndOfStream; ++lineIndex)
        {
            string line = sr.ReadLine()!;
            schematic.Add(line);
            for (int i = 0; i < line.Length; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    nps.Add(new(line,ref i,lineIndex));
                }
            }
        }
        int sum = 0;
        foreach (var np in nps)
        {
            if (IsValidNp(np, schematic))
                sum += np.Number;
        }
        Console.WriteLine("Sum: "+sum);
    }

    static int GearRatio(PossibleGear pg, List<NumberPosition> nps)
    {
        List<NumberPosition> connectedNps = [];
        foreach (var np in nps)
        {
            if ( pg.Line >= np.LineIndex -1 && pg.Line <= np.LineIndex+1
                && pg.Index >= np.StartIndex -1 && pg.Index <= np.EndIndex)
                {
                    connectedNps.Add(np);
                }
        }
        if (connectedNps.Count != 2) return 0;
        var res = connectedNps[0].Number * connectedNps[1].Number;
        return res;
    }

    static void PartTwo(StreamReader sr)
    {
        Console.WriteLine("Part Two");
        List<string> schematic = [];

        List<NumberPosition> nps = [];
        List<PossibleGear> pgs = [];

        for (int lineIndex = 0; !sr.EndOfStream; ++lineIndex)
        {
            string line = sr.ReadLine()!;
            schematic.Add(line);
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '*')
                {
                    pgs.Add(new (lineIndex, i));
                }
                if (char.IsDigit(line[i]))
                {
                    nps.Add(new(line,ref i,lineIndex));
                }
            }
        }

        var validNps = nps.Where(np => IsValidNp(np,schematic)).ToList();
        int sum = pgs.Sum(pg => GearRatio(pg,validNps));

        Console.WriteLine("Sum: "+sum);
    }
}