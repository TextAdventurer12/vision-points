using System;
using VisionPoints.Preprocessing;

namespace VisionPoints.Skills
{
    class BurstSpeed : Skill
    {
        protected override double skillMultiplier => 1;
        public override void Process(DifficultyObject obj, List<Skill> processedSkills)
        {
            double difficulty = 1 / obj.StrainTime;
            difficulties.Add(difficulty);
        }
        public override double DifficultyValue()
        {
            double difficulty = 0;
            for (int i = 0; i < difficulties.Count(); i++)
                difficulty += difficulties[i] / (i + 1);
            return difficulty * skillMultiplier;
        }
    }
}