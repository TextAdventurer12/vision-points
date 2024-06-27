using System;
using VisionPoints.Preprocessing;
using VisionPoints.Skills;

namespace VisionPoints
{
   abstract class Skill
    {
        public List<double> difficulties = new List<double>();

        protected virtual double skillMultiplier => 0;

        protected double? cachedDifficulty;

        public double LastDifficulty => difficulties.Last();

        public abstract void Process(DifficultyObject obj, SkillsHandler skills);

        protected abstract double DifficultyValue();

        public double Difficulty
        {
            get => cachedDifficulty == null ? (cachedDifficulty = DifficultyValue()).Value : cachedDifficulty.Value;
            set => cachedDifficulty = value;
        }
    }
}