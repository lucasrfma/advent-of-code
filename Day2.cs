using Microsoft.Extensions.Logging;

namespace Aoc;

public class Day2
{
    class CubeSet
    {
        internal int Red { get; set; }
        internal int Green { get; set; }
        internal int Blue { get; set; }

        internal bool Fits(CubeSet cubeSet)
        {
            return Red >= cubeSet.Red && Blue >= cubeSet.Blue && Green >= cubeSet.Green;
        }
    }

    class GameInfo()
    {
        internal int Id { get; }
        private CubeSet _maxByColor = new () {Red = 0, Green = 0, Blue = 0 };
        internal CubeSet MaxByColor { get => _maxByColor;
            set
        {
            _maxByColor.Red = value.Red > _maxByColor.Red ? value.Red : _maxByColor.Red;
            _maxByColor.Green = value.Green > _maxByColor.Green ? value.Green : _maxByColor.Green;
            _maxByColor.Blue = value.Blue > _maxByColor.Blue ? value.Blue : _maxByColor.Blue;
        } }

        internal GameInfo(string line) : this()
        {
            string[] idAndPlays = line.Split(':');
            Id = int.Parse(idAndPlays[0].Split(' ')[1]);
            string[] plays = idAndPlays[1].Split(';');

            foreach (var play in plays)
            {
                MaxByColor = ParsePlay(play);
            }
        }

        internal CubeSet ParsePlay(string play)
        {
            string[] colorsInfo = play.Split(',');
            CubeSet cubeSet = new() {Red = 0, Green = 0, Blue = 0 };
            foreach (var colorInfo in colorsInfo)
            {
                string[] numberAndColor = colorInfo.Trim().Split(' ');

                int number = int.Parse(numberAndColor[0]);
                string color = numberAndColor[1];
                switch (color[0])
                {
                    case 'r': cubeSet.Red = number; break;
                    case 'g': cubeSet.Green = number; break;
                    case 'b': cubeSet.Blue = number; break;
                }
            }
            return cubeSet;
        }
    }

    public static void PartOne(StreamReader sr, ILogger logger)
    {
        CubeSet bag = new() {Red = 12, Green = 13, Blue = 14 };
        int sum = 0;

        while (!sr.EndOfStream)
        {
            GameInfo gi = new GameInfo(sr.ReadLine()!);
            if (bag.Fits(gi.MaxByColor))
                sum += gi.Id;
        }
        logger.LogInformation("ID Sum: "+sum);
    }

    public static void PartTwo(StreamReader sr, ILogger logger)
    {
        int sum = 0;

        while (!sr.EndOfStream)
        {
            GameInfo gi = new GameInfo(sr.ReadLine()!);
            sum += gi.MaxByColor.Red * gi.MaxByColor.Blue * gi.MaxByColor.Green;
        }
        logger.LogInformation("ID Sum: "+sum);
    }
}