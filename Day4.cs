using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day4(StreamReader sr, ILogger logger)
{
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

    public void PartOne()
    {
        logger.LogInformation("Part One");
        
        List<Card> cards = [];
        while(!sr.EndOfStream)
        {
            string line = sr.ReadLine()!;
            cards.Add(new Card(line));
        }
        int sum = cards.Sum(c => c.CardValue());
        logger.LogInformation("Sum: "+sum);
    }

    public void PartTwo()
    {
        logger.LogInformation("Part Two");
        
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
                    cards[addingIndex].Count += cards[i].Count;
            }
        }
        int sum = cards.Sum(c => c.Count);
        logger.LogInformation("Sum: "+sum);
    }
    
}