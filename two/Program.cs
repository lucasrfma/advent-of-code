namespace Two;

class Program
{
    class CubeSet()
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
        internal int Id { get; set; }
        private CubeSet _maxByColor = new () {Red = 0, Green = 0, Blue = 0 };
        internal CubeSet MaxByColor { get {return this._maxByColor; } 
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
                    default: break;
                }
            }
            return cubeSet;
        }
    }

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

    static void PartOne(StreamReader sr)
    {
        CubeSet bag = new() {Red = 12, Green = 13, Blue = 14 };
        int sum = 0;

        while (!sr.EndOfStream)
        {
            GameInfo gi = new GameInfo(sr.ReadLine()!);
            if (bag.Fits(gi.MaxByColor))
                sum += gi.Id;
        }
        Console.WriteLine("ID Sum: "+sum);
    }

    static void PartTwo(StreamReader sr)
    {
        int sum = 0;

        while (!sr.EndOfStream)
        {
            GameInfo gi = new GameInfo(sr.ReadLine()!);
            sum += gi.MaxByColor.Red * gi.MaxByColor.Blue * gi.MaxByColor.Green;
        }
        Console.WriteLine("ID Sum: "+sum);
    }
}