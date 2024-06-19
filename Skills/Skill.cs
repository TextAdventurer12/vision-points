using System;
using VisionPoints.Preprocessing;

namespace VisionPoints
{
   abstract class Skill
    {
        protected List<double> difficulties = new List<double>();
        protected virtual double skillMultiplier => 0;
        public double LastDifficulty => difficulties.Last();
        public abstract void Process(DifficultyObject obj, List<Skill> processedSkills);
        public abstract double DifficultyValue();
    }
}