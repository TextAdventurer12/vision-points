using System;
using System.Numerics;
using VisionPoints.Preprocessing;
using VisionPoints.Utils;
using ZstdSharp.Unsafe;

namespace VisionPoints.Skills
{
    class FlowAim : OsuTimeSkill
    {
        protected override double skillMultiplier => 1;

        /// <summary>
        /// This is an aim skill, and therefore represents the difficulty of getting your cursor to the centre of the next object
        /// 'Flow' aim is aim in which the cursor's velocity is maintained and rotated or scaled to manuever the cursor between objects
        /// Flow aim difficulty = velocity + change in rotation + acceleration
        /// </summary>
        /// <param name="obj">The object to evaluate the difficulty of</param>
        /// <param name="skills"></param>
        /// <returns></returns>
        protected override double DifficultyAt(DifficultyObject obj, SkillsHandler skills)
        {
            DifficultyObject? previous = obj.Previous(1);
            if (previous is null)
                return 0;

            double difficulty = 0;

            Vector2 velocity = obj.TravelVelocity;
            difficulty += velocity.Length();
            Vector2 previousVelocity = previous.TravelVelocity;
            double angleChange = Math.Abs(velocity.Angle() - previousVelocity.Angle()) / (Math.PI * 2);
            difficulty *= 1 + angleChange;

            double velocityChange = velocity.LengthSquared() > previousVelocity.LengthSquared() ? velocity.Length() / previousVelocity.Length() : previousVelocity.Length() / velocity.Length();
            difficulty *= velocityChange;

            // If it is easier to snap this object than to flow it, ignore flow aim difficulty
            if (difficulty > skills.SnapAim)
                return 0;
            // This note should be flowed, so remove its snap aim difficulty
            skills.snapAim.FlowLastNote();

            return difficulty;
        }
        protected override double HitProbability(double skill, double difficulty)
        {
            if (skill <= 0) return 0;
            if (difficulty <= 0) return 1;

            return SpecialFunctions.Erf(skill / (Math.Sqrt(2) * difficulty));
        }
    }
}