using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day10(StreamReader sr, ILogger logger)
{
    public void PartOne()
    {
        logger.LogInformation("Part One");

        List<string> map = [];

        var ConnectedDirections = new Dictionary<char,(bool up, bool right, bool down, bool left)>
        {
            { '|', (up: true, right: false, down: true, left: false)},
            { '-', (up: false, right: true, down: false, left: true)},
            { 'L', (up: true, right: true, down: false, left: false)},
            { 'J', (up: true, right: false, down: false, left: true)},
            { '7', (up: false, right: false, down: true, left: true)},
            { 'F', (up: false, right: true, down: true, left: false)},
            { '.', (up: false, right: false, down: false, left: false)},
            { 'S', (up: true, right: true, down: true, left: true)},
        };
        
        while (!sr.EndOfStream)
        {
            map.Add(sr.ReadLine()!);
        }

        (int i, int j) s = FindS();
        List<(int i, int j)> wrongNodes = [];
        List<(int i, int j)> correctPath = [];
        (int i, int j) currentNode = s;
        char currentPipe = map[currentNode.i][currentNode.j];
        int count = 0;
        while (true)
        {
            Console.Write(currentPipe + (++count % 25 == 0 ? "\n" : " "));
            var possibleMovements = PossibleMovements(currentNode.i,currentNode.j);
            var numberOfPm = possibleMovements.Aggregate(0, (ctr, b) => b.can ? ++ctr : ctr);
            if (numberOfPm < 2)
            {
                logger.LogInformation("Dead end, going back to S");
                wrongNodes.AddRange(correctPath);
                correctPath = [];
                currentNode = s;
                currentPipe = map[currentNode.i][currentNode.j];
                count = 0;
                continue;
            }

            currentNode = possibleMovements.Find(b => b.can &&
                                !correctPath.Contains(b.to) && !(count <= 2 && b.to == s)
                ).to;
            currentPipe = map[currentNode.i][currentNode.j];
            
            if (currentPipe == 'S') break;
            correctPath.Add(currentNode);
            
        }

        int distance = correctPath.Count / 2 + 1;
        var far = correctPath[correctPath.Count / 2];
        var farPipe = map[far.i][far.j];
         
        Console.WriteLine($"The farthest pipe is {farPipe} at ({far.i},{far.j}). Distance: {distance}");
        
        List<(bool can,(int i, int j) to)> PossibleMovements(int i, int j) => [
            CanGoUp(i,j),
            CanGoRight(i,j),
            CanGoDown(i,j),
            CanGoLeft(i,j),
        ];

        (bool can, (int i, int j) to) CanGoUp(int i, int j)
        {
            (int i, int j) to = (i - 1, j);
            bool isWrong = wrongNodes.Contains(to);
            return  (i > 0 &&
                        ConnectedDirections[currentPipe].up &&
                        ConnectedDirections[map[to.i][to.j]].down &&
                        !wrongNodes.Contains(to),
                    to);
        }

        (bool can, (int i, int j) to) CanGoRight(int i, int j)
        {
            (int i, int j) to = (i, j + 1);
            return  (j < map[i].Length - 1 &&
                        ConnectedDirections[currentPipe].right &&
                        ConnectedDirections[map[to.i][to.j]].left &&
                        !wrongNodes.Contains(to),
                    to);
        }

        (bool can, (int i, int j) to) CanGoDown(int i, int j)
        {
            (int i, int j) to = (i + 1, j);
            return  (i < map.Count - 1 &&
                        ConnectedDirections[currentPipe].down &&
                        ConnectedDirections[map[to.i][to.j]].up &&
                        !wrongNodes.Contains(to),
                    to);
        }

        (bool can, (int i, int j) to) CanGoLeft(int i, int j)
        {
            (int i, int j) to = (i, j - 1);
            return  (j > 0 &&
                ConnectedDirections[currentPipe].left &&
                ConnectedDirections[map[to.i][to.j]].right &&
                !wrongNodes.Contains(to),
                to);
        }
        
        (int i, int j) FindS()
        {
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'S') return (i, j);
                }
            }
            throw new Exception("There should always be an S in the input!");
        }
    }
    

    public void PartTwo()
    {
        logger.LogInformation("Part Two");

    }
}