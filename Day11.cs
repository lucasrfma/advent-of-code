using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day11(StreamReader sr, ILogger logger)
{
    private const char EmptySpace = '.';
    public void PartOne()
    {
        logger.LogInformation("Part One");
        Solution(2);
    }

    public void PartTwo()
    {
        logger.LogInformation("Part Two");
        Solution(1_000_000);
    }

    private void Solution(int expansion)
    {
        List<List<char>> galaxyMap = [];

        List<int> columnsWithoutGalaxies = [];
        List<int> linesWithoutGalaxies = [];
        List<(int i, int j)> galaxyPositions = [];

        int i = 0;
        while (!sr.EndOfStream)
        {
            galaxyMap.Add([]);
            int j = 0;
            bool emptyLine = true;
            int c;
            for (c = sr.Read(); c is not '\r'
                 and not -1; c = sr.Read())
            {
                galaxyMap[i].Add((char)c);
                if (c is not EmptySpace)
                {
                    emptyLine = false;
                    galaxyPositions.Add((i,j));
                }
                ++j;
            }
            if (emptyLine)
            {
                linesWithoutGalaxies.Add(i);
            }
            if (c != -1)
            {
                sr.ReadLine();
                ++i;
            }
        }

        for (int j = 0; j < galaxyMap[0].Count; j++)
        {
            bool emptyCol = true;
            for (i = 0; i < galaxyMap.Count; i++)
            {
                if (galaxyMap[i][j] is not EmptySpace) emptyCol = false;
            }
            if (emptyCol) columnsWithoutGalaxies.Add(j);
        }

        long minSum = 0;
        for (int one = 0; one < galaxyPositions.Count; one++)
        {
            for (int other = one + 1; other < galaxyPositions.Count; other++)
            {
                int oneI = galaxyPositions[one].i;
                int otherI = galaxyPositions[other].i;
                var (minI, maxI) = otherI < oneI ? (otherI, oneI) : (oneI, otherI);
                
                int emptyLinesCount = linesWithoutGalaxies.Count(li => li > minI && li < maxI);
                long distanceI = maxI - minI + (expansion-1) * emptyLinesCount;
                
                int oneJ = galaxyPositions[one].j;
                int otherJ = galaxyPositions[other].j;
                var (minJ, maxJ) = otherJ < oneJ ? (otherJ, oneJ) : (oneJ, otherJ);
                
                int emptyColumnsCount = columnsWithoutGalaxies.Count(lj => lj > minJ && lj < maxJ);
                long distanceJ = maxJ - minJ + (expansion-1) * emptyColumnsCount;

                minSum += distanceI + distanceJ;
            }
        }
        Console.WriteLine($"Min distance sum: {minSum}");
    }
}