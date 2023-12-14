using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day7(StreamReader sr, ILogger logger)
{
    [Flags]
    internal enum Value: long
    {
        C2 = 1L,
        C3 = 2L,
        C4 = 3L,
        C5 = 4L,
        C6 = 5L,
        C7 = 6L,
        C8 = 7L,
        C9 = 8L,
        CT = 9L,
        CJ = 10L,
        CQ = 11L,
        CK = 12L,
        CA = 13L,
        HighCard = 0,
        OnePair = 1L << 40,
        TwoPairs = 1L << 41,
        ThreeOfAKind = 1L << 42,
        FullHouse = ThreeOfAKind | OnePair,
        FourOfAKind = 1L << 44,
        FiveOfAKind = 1L << 45,
    }

    internal struct Hand
    {
        internal char[] Cards;
        internal long Bid;
        internal long TotalValue {get;init;}

        internal Hand(string cards, long bid)
        {
            Cards = cards.ToCharArray();
            Bid = bid;
            TotalValue = HandType() + IndividualCardsValue();
        }

        internal long HandType()
        {
            char[] copiedCards = Cards.Order().ToArray();
            Dictionary<char,int> cardCounts = new Dictionary<char, int>();
            foreach (var c in copiedCards)
            {
                if (cardCounts.TryGetValue(c,out int currentValue))
                    cardCounts[c] = currentValue + 1;
                else
                    cardCounts.Add(c,1);
            }
            return cardCounts.Aggregate((long)Value2.HighCard, (s, keyValuePair) => keyValuePair switch
            {
                (_,2) => s + (long) Value.OnePair,
                (_,3) => s + (long) Value.ThreeOfAKind,
                (_,4) => s + (long) Value.FourOfAKind,
                (_,5) => s + (long) Value.FiveOfAKind,
                _ => s + (long) Value.HighCard
            });
        }

        internal long IndividualCardsValue()
        {
            long icv = 0;
            long mult = 1 << 24;
            foreach (var card in Cards)
            {
                icv += CardValueMap(card) * mult;
                mult >>= 4;
            }
            return icv;
        }

        internal long CardValueMap(char c) =>
        c switch
        {
            '2' => (long) Value.C2,
            '3' => (long) Value.C3,
            '4' => (long) Value.C4,
            '5' => (long) Value.C5,
            '6' => (long) Value.C6,
            '7' => (long) Value.C7,
            '8' => (long) Value.C8,
            '9' => (long) Value.C9,
            'T' => (long) Value.CT,
            'J' => (long) Value.CJ,
            'Q' => (long) Value.CQ,
            'K' => (long) Value.CK,
            'A' => (long) Value.CA,
            _ => 0
        };
    }

    [Flags]
    internal enum Value2: long
    {
        CJ = 1L,
        C2 = 2L,
        C3 = 3L,
        C4 = 4L,
        C5 = 5L,
        C6 = 6L,
        C7 = 7L,
        C8 = 8L,
        C9 = 9L,
        CT = 10L,
        CQ = 11L,
        CK = 12L,
        CA = 13L,
        HighCard = 0,
        OnePair = 1L << 40,
        TwoPairs = 1L << 41,
        ThreeOfAKind = 1L << 42,
        FullHouse = ThreeOfAKind | OnePair,
        FourOfAKind = 1L << 44,
        FiveOfAKind = 1L << 45,
    }

    internal struct Hand2
    {
        internal char[] Cards;
        internal long Bid;
        internal long TotalValue {get;init;}

        internal Hand2(string cards, long bid)
        {
            Cards = cards.ToCharArray();
            Bid = bid;
            TotalValue = HandType() + IndividualCardsValue();
        }

        internal long HandType()
        {
            char[] copiedCards = Cards.Order().ToArray();

            Dictionary<char,int> cardCounts = new Dictionary<char, int>();
            foreach (var c in copiedCards)
            {
                if (cardCounts.TryGetValue(c,out int currentValue))
                    cardCounts[c] = currentValue + 1;
                else
                    cardCounts.Add(c,1);
            }
            if (cardCounts.TryGetValue('J',out int jCount))
                cardCounts.Remove('J');

            int min = 2;
            List<char> boostCandidates = [];
            foreach (var key in cardCounts.Keys)
            {
                int c = cardCounts[key];
                if (c == min)
                    boostCandidates.Add(key);
                else if (c > min)
                {
                    boostCandidates = [key];
                    min = c;
                }
            }

            if (boostCandidates.Count > 0)
            {
                char biggestValue = boostCandidates.MaxBy(CardValueMap);
                cardCounts[biggestValue] += jCount;
            }
            else if (cardCounts.Keys.Count > 0)
                cardCounts[cardCounts.Keys.First()] += jCount;
            else
                cardCounts.Add('J',jCount);


            return cardCounts.Aggregate((long)Value2.HighCard, (s, keyValuePair) => keyValuePair switch
                {
                    (_,2) => s + (long) Value2.OnePair,
                    (_,3) => s + (long) Value2.ThreeOfAKind,
                    (_,4) => s + (long) Value2.FourOfAKind,
                    (_,5) => s + (long) Value2.FiveOfAKind,
                    _ => s + (long) Value2.HighCard
                });
        }

        internal long IndividualCardsValue()
        {
            long icv = 0;
            int shift = 24;
            foreach (var card in Cards)
            {
                icv += CardValueMap(card) << shift;
                shift -= 4;
            }
            return icv;
        }

        internal long CardValueMap(char c) =>
            c switch
        {
            '2' => (long) Value2.C2,
            '3' => (long) Value2.C3,
            '4' => (long) Value2.C4,
            '5' => (long) Value2.C5,
            '6' => (long) Value2.C6,
            '7' => (long) Value2.C7,
            '8' => (long) Value2.C8,
            '9' => (long) Value2.C9,
            'T' => (long) Value2.CT,
            'J' => (long) Value2.CJ,
            'Q' => (long) Value2.CQ,
            'K' => (long) Value2.CK,
            'A' => (long) Value2.CA,
            _ => 0
        };
    }

    public void PartOne()
    {
        logger.LogInformation("Part One");
        List<Hand> hands = [];
        while (!sr.EndOfStream)
        {
            string[] line = sr.ReadLine()!.Split(' ');
            hands.Add(new (line[0],int.Parse(line[1])));
        }
        List<Hand> rankedHands = hands.OrderBy(h => h.TotalValue).ToList();
        long total = 0;
        for (int i = 0; i < rankedHands.Count; i++)
        {
            long bid = rankedHands[i].Bid;
            int rank = i+1;
            long prize = bid*rank;

            total += prize;
        }
        logger.LogInformation("Total: "+total);
    }

    public void PartTwo()
    {
        logger.LogInformation("Part Two");
        List<Hand2> hands = [];
        while (!sr.EndOfStream)
        {
            string[] line = sr.ReadLine()!.Split(' ');
            hands.Add(new (line[0],int.Parse(line[1])));
        }
        List<Hand2> rankedHands = hands.OrderBy(h => h.TotalValue).ToList();

        long total = 0;
        for (int i = 0; i < rankedHands.Count; i++)
        {
            long bid = rankedHands[i].Bid;
            int rank = i+1;
            long prize = bid*rank;

            total += prize;
        }
        logger.LogInformation("Total: "+total);
    }
}