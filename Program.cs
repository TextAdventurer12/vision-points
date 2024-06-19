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
            Console.WriteLine(CalculatePerformance(diffObjects));
        }
        public static double CalculatePerformance(IEnumerable<DifficultyObject> difficultyObjects)
        {
            Skill[] skills = { new SnapAim(), };
            foreach (var difficultyObject in difficultyObjects)
            {
                List<Skill> processedSkills = new List<Skill>();
                foreach (Skill skill in skills)
                {
                    skill.Process(difficultyObject, processedSkills);
                    processedSkills.Add(skill);
                }
            }
            return skills.Sum(skill => skill.DifficultyValue());
        }
    }
}