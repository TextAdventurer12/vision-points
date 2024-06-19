using System.Collections.Generic;
using VisionPoints.Skills;
using VisionPoints.Preprocessing;

namespace VisionPoints
{
    class VisionPointsCalculator
    {
        public double SnapAim;
        public double Total;
        public VisionPointsCalculator(double SnapAim, double Total)
        {
            this.SnapAim = SnapAim;
            this.Total = Total;
        }
        public static VisionPointsCalculator CalculatePerformance(IEnumerable<DifficultyObject> difficultyObjects)
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
            return new VisionPointsCalculator(
                skills[0].DifficultyValue(),
                skills.Sum(skill => skill.DifficultyValue())
            );
        }
    }
}