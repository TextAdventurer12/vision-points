using System;
using System.Numerics;
using VisionPoints.Preprocessing;
using VisionPoints.Utils;

namespace VisionPoints.Skills
{
    class SnapAim : OsuTimeSkill
    {
        protected override double skillMultiplier => 1;

        /// <summary>
        /// This is an aim skill, and therefore represents the difficulty of getting your cursor to the centre of the next object
        /// 'Snap' aim is aim in which the velocity of each movement is cancelled out with a snap movement on top of each circle
        /// Snap aim difficulty = magnitude of decceleration needed for the snap + magnitude of acceleration change needed to reach the next velocity + magnitude of velocity
        /// </summary>
        /// <param name="obj">The object to evaluate the difficulty of</param>
        /// <param name="skills">The skills that have already been processed. This is the first calculated skill, so it should not be used</param>
        /// <returns></returns>
        protected override double DifficultyAt(DifficultyObject obj, SkillsHandler skills)
        {
            DifficultyObject? previous = obj.Previous(1);
            if (previous is null)
                return 0;
            DifficultyObject? next = obj.Next(1);
            if (next is null)
                return 0;

            double difficulty = 0;

            // We need to counter the previous velocity to snap onto this object
            Vector2 prevVelocityTravel = obj.TravelVelocity;
            // How fast do we need to deccelerate onto this object
            Vector2 deccelVector =  Vector2.Divide(-prevVelocityTravel, (Single)0.1);
            difficulty += deccelVector.Length(); // Faster decceleration = harder

            // We need to accelerate to the next velocity
            // What acceleration must be done
            Vector2 nextAcceleration = Vector2.Divide(next.TravelVelocity, (Single)0.1);
            // Find the change in acceleration needed from the snap movement to the velocity movement
            Vector2 deltaAcceleration = nextAcceleration - deccelVector;
            difficulty += deltaAcceleration.Length();

            // Increase difficulty by velocity
            difficulty += prevVelocityTravel.Length();

            return difficulty;
        }
        protected override double HitProbability(double skill, double difficulty)
        {
            if (skill <= 0) return 0;
            if (difficulty <= 0) return 1;

            return SpecialFunctions.Erf(skill / (Math.Sqrt(2) * difficulty));
        }
        /// <summary>
        /// The last note is flow aim, and so the difficulty should be accounted for from the FlowAim skill
        /// </summary>
        public void FlowLastNote()
        {
            difficulties[difficulties.Count() - 1] = 0;
        }
    }
}