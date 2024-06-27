using System;

namespace VisionPoints.Skills
{
    class SkillsHandler
    {
        public SnapAim snapAim;
        public double SnapAim => snapAim.difficulties.Last();
        public FlowAim flowAim;
        public double FlowAim => flowAim.difficulties.Last();
        public SkillsHandler(SnapAim snapAim, FlowAim flowAim)
        {
            this.snapAim = snapAim;
            this.flowAim = flowAim;
        }
        public List<Skill> ToList()
        {
            return new List<Skill>()
            {
                snapAim,
                flowAim,
            };
        }
    }
}