using System;
using System.Collections.Generic;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Decoders;
using VisionPoints.Skills;
using VisionPoints.Preprocessing;

namespace VisionPoints
{
    class Program
    {
        public static void Main(string[] args)
        {
            Beatmap beatmap = BeatmapDecoder.Decode("beatmaps/652412 Hanasaka Yui(CV M.A.O) - Harumachi Clover/Hanasaka Yui(CV M.A.O) - Harumachi Clover (ezek) [Normal].osu");
            IEnumerable<DifficultyObject> diffObjects = DifficultyObject.CreateDifficultyObjects(beatmap.HitObjects);
            Console.WriteLine(VisionPointsCalculator.CalculatePerformance(diffObjects).Total);
        }
    }
}