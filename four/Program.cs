internal class Program
{
    private static void Main()
    {
        // I decided to not read console args anymore.
        string input = "input.txt";

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

//        PartOne(sr);
        PartTwo(sr);

    }
    
    class Card()
    {
        internal int Id { get; set; }
        internal int[] Winning { get; }
        internal int MatchCount { get; }
        internal int Count { get; set; } = 1;
        
        internal int CardValue()
        {
            if (MatchCount == 0) return 0;
            return 1 << (MatchCount - 1);
        }
        
        internal Card(string cardInfo) : this()
        {
            string[] headerNumbers = cardInfo.Split(':');
            Id = int.Parse(headerNumbers[0].Split(' ',StringSplitOptions.RemoveEmptyEntries)[1]);
            
            string[] winningOwned = headerNumbers[1].Split('|');
            Winning = winningOwned[0].Split(' ',StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();
            MatchCount = winningOwned[1].Split(' ',StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).Sum(i => {
                if (Winning.Contains(i))
                    return 1;
                return 0;
                });
        }
    }

    static void PartOne(StreamReader sr)
    {
        Console.WriteLine("Part One");
        
        List<Card> cards = [];
        while(!sr.EndOfStream)
        {
            string line = sr.ReadLine()!;
            cards.Add(new Card(line));
        }
        int sum = cards.Sum(c => c.CardValue());
        Console.WriteLine("Sum: "+sum);
    }

    static void PartTwo(StreamReader sr)
    {
        Console.WriteLine("Part Two");
        
        List<Card> cards = [];
        while(!sr.EndOfStream)
        {
            string line = sr.ReadLine()!;
            cards.Add(new Card(line));
        }
        for (int i = 0; i < cards.Count - 1; ++i)
        {
            for (int j = 0; j < cards[i].MatchCount; ++j)
            {
                int addingIndex = i + j + 1;
                if (addingIndex < cards.Count)
                    cards[i+j+1].Count += cards[i].Count;
            }
        }
        int sum = cards.Sum(c => c.Count);
        Console.WriteLine("Sum: "+sum);
    }
    
}