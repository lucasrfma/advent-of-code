internal class Program
{
    private static void Main(string[] args)
    {
        // I decided to not read console args anymore.
        string input = "ex-input.txt";

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

        PartOne(sr);
        // PartTwo(sr);
    }

    static void PartOne(StreamReader sr)
    {
        Console.WriteLine("Part One");
        while(!sr.EndOfStream)
        {
            string line = sr.ReadLine()!;
        }
    }

    static void PartTwo(StreamReader sr)
    {
        Console.WriteLine("Part Two");
        while(!sr.EndOfStream)
        {
            string line = sr.ReadLine()!;
        }
    }

}