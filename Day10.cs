using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day10(StreamReader sr, ILogger logger)
{
    private enum Direction
    {
        Northward,
        Eastward,
        Southward,
        Westward
    }

    private enum Tile
    {
        Undefined,
        Outside,
        Inside,
        Perimeter
    }

    private static Direction CheckDirection((int i, int j) newNode, (int i, int j) prevNode)
    {
        var delta = (newNode.i - prevNode.i,newNode.j - prevNode.j);
        return delta switch
        {
            (-1, 0) => Direction.Northward,
            (0, 1) => Direction.Eastward,
            (1, 0) => Direction.Southward,
            (0, -1) => Direction.Westward,
            _ => throw new Exception("No diagonals possible!")
        };
    }

    public void PartOne()
    {
        logger.LogInformation("Part One and Two");

        List<string> map = [];

        var ConnectedDirections = new Dictionary<char, (bool north, bool east, bool south, bool west)>
        {
            { '|', (north: true, east: false, south: true, west: false) },
            { '-', (north: false, east: true, south: false, west: true) },
            { 'L', (north: true, east: true, south: false, west: false) },
            { 'J', (north: true, east: false, south: false, west: true) },
            { '7', (north: false, east: false, south: true, west: true) },
            { 'F', (north: false, east: true, south: true, west: false) },
            { '.', (north: false, east: false, south: false, west: false) },
            { 'S', (north: true, east: true, south: true, west: true) },
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
            Console.Write(currentPipe + (++count % 140 == 0 ? "\n" : ""));
            var possibleMovements = PossibleMovements(currentNode.i, currentNode.j);
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

        // I don't know whats inside and whats outside at the start
        List<List<Tile>> tileMap = [];
        for (int i = 0; i < map.Count; i++)
        {
            tileMap.Add([]);
            for (int j = 0; j < map[i].Length; j++)
            {
                tileMap[i].Add(Tile.Undefined);
            }
        }

        // S + path are part of the perimeter
        tileMap[s.i][s.j] = Tile.Perimeter;
        foreach (var node in correctPath)
        {
            tileMap[node.i][node.j] = Tile.Perimeter;
        }


        foreach (var t in tileMap)
        {
            foreach (var t1 in t)
            {
                Console.Write(t1 switch
                {
                    Tile.Outside => "O",
                    Tile.Inside => "I",
                    Tile.Undefined => " ",
                    _ => "P"
                });
            }
            Console.WriteLine();
        }


        // I'll first convert S node into a regular pipe node, because S doesn't give us information by itself.
        var firstNodeDir = CheckDirection(correctPath.First(), s);
        var lastNodeDir = CheckDirection(correctPath.Last(), s);
        // this switch can be abridged since we know the order of path testing is N->E->S->W
        var sFormat = (firstNodeDir, lastNodeDir) switch
        {
            (Direction.Northward, Direction.Eastward) => 'L',
            (Direction.Northward, Direction.Southward) => '|',
            (Direction.Northward, Direction.Westward) => 'J',
            (Direction.Eastward, Direction.Southward) => 'F',
            (Direction.Eastward, Direction.Westward) => '-',
            (Direction.Southward, Direction.Westward) => '7',
        };

        map[s.i] = map[s.i].Replace('S', sFormat);

        // We scan left->right, each tile found is 'outside' until with find a part of the perimeter.
        // We need to then check the shape of the pipe.
        // If '|', we flip the assumption of the next tiles ('outside -> 'Inside' or vice versa.)
        // If it's 'F' or 'L', we gotta check the last pipe of this sequence to see if we flip the assumption or not.
        // i.e.: 'F' followed by 'J', or 'L' followed by '7' -> flip, otherwise don't flip.
        Console.WriteLine();
        int insideCount = 0;
        for (int i = 0; i < tileMap.Count; i++)
        {
            Tile tile = Tile.Outside;
            for (int j = 0; j < tileMap[i].Count; j++)
            {
                if (tileMap[i][j] == Tile.Perimeter)
                {
                    var foundPipe = map[i][j];
                    switch (foundPipe)
                    {
                        case '|':
                            tile = tile == Tile.Outside ? Tile.Inside : Tile.Outside;
                            break;
                        case 'F' or 'L':
                        {
                            var flipCondition = foundPipe == 'F' ? 'J' : '7';
                            var noFlipCondition = foundPipe == 'F' ? '7' : 'J';
                            while (++j < tileMap[i].Count)
                            {
                                foundPipe = map[i][j];
                                if (foundPipe == flipCondition)
                                {
                                    tile = tile == Tile.Outside ? Tile.Inside : Tile.Outside;
                                    break;
                                }
                                if (foundPipe == noFlipCondition)
                                {
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    tileMap[i][j] = tile;
                    if (tile == Tile.Inside) ++insideCount;
                }
            }
        }

        foreach (var t in tileMap)
        {
            foreach (var t1 in t)
            {
                Console.Write(t1 switch
                {
                    Tile.Outside => "O",
                    Tile.Inside => "I",
                    // Tile.Undefined => " ",
                    _ => " "
                });
            }
            Console.WriteLine();
        }

        Console.WriteLine($"There are {insideCount} tiles inside the pipe loop!");
        return;

        List<(bool can,(int i, int j) to)> PossibleMovements(int i, int j) => [
            CanGoUp(i,j),
            CanGoRight(i,j),
            CanGoDown(i,j),
            CanGoLeft(i,j),
        ];

        (bool can, (int i, int j) to) CanGoUp(int i, int j)
        {
            (int i, int j) to = (i - 1, j);
            return  (i > 0 &&
                        ConnectedDirections[currentPipe].north &&
                        ConnectedDirections[map[to.i][to.j]].south &&
                        !wrongNodes.Contains(to),
                    to);
        }

        (bool can, (int i, int j) to) CanGoRight(int i, int j)
        {
            (int i, int j) to = (i, j + 1);
            return  (j < map[i].Length - 1 &&
                        ConnectedDirections[currentPipe].east &&
                        ConnectedDirections[map[to.i][to.j]].west &&
                        !wrongNodes.Contains(to),
                    to);
        }

        (bool can, (int i, int j) to) CanGoDown(int i, int j)
        {
            (int i, int j) to = (i + 1, j);
            return  (i < map.Count - 1 &&
                        ConnectedDirections[currentPipe].south &&
                        ConnectedDirections[map[to.i][to.j]].north &&
                        !wrongNodes.Contains(to),
                    to);
        }

        (bool can, (int i, int j) to) CanGoLeft(int i, int j)
        {
            (int i, int j) to = (i, j - 1);
            return  (j > 0 &&
                ConnectedDirections[currentPipe].west &&
                ConnectedDirections[map[to.i][to.j]].east &&
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
        logger.LogInformation("Implemented inside part 1");
    }
}