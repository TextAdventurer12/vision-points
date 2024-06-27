using System.Collections.Generic;
using VisionPoints.Skills;
using VisionPoints.Preprocessing;

namespace VisionPoints
{
    class VisionPointsCalculator
    {
        public double SnapAim;
        public double FlowAim;
        public double Total;
        public static VisionPointsCalculator CalculatePerformance(List<DifficultyObject> difficultyObjects)
        {
            SkillsHandler skills = new SkillsHandler(new SnapAim(), new FlowAim());
            foreach (var difficultyObject in difficultyObjects)
            {
                foreach (Skill skill in skills.ToList())
                {
                    skill.Process(difficultyObject, skills);
                }
            }
            return new VisionPointsCalculator()
            {
                SnapAim = skills.snapAim.Difficulty,
                FlowAim = skills.flowAim.Difficulty,
                Total = skills.ToList().Sum(skill => skill.Difficulty)
            };
        }
    }
}