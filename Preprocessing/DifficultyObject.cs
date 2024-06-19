using System;
using OsuParsers.Beatmaps.Objects;
using System.Numerics;

namespace VisionPoints.Preprocessing
{
    class DifficultyObject
    {
        /// <summary>
        /// The raw vector between the center of the previous object and the centre of this object
        /// </summary>
        public readonly Vector2 Travel;
        /// <summary>
        /// The raw osu px distance between the centre of the previous object and the centre of this object
        /// </summary>
        public double Distance => Travel.Length();
        /// <summary>
        /// The time in ms from the end of the previous object to the start of this object
        /// </summary>
        public readonly double StrainTime;

        /// <summary>
        /// The angle formed between this object, the previous object, and the object before the previous object
        /// </summary>
        public double Angle;
        
        /// <summary>
        /// StrainTime does not exceed this figure
        /// temporary to prevent any div by zero errors
        /// </summary>
        const double min_strain_time = 1;

        /// <summary>
        /// The velocity from the previous object to this object
        /// </summary>
        public double Velocity => Distance / StrainTime;

        public DifficultyObject(HitObject current, HitObject? previous, HitObject? previousPrevious)
        {
            if (previous is null)
            {
                this.Travel = new Vector2(0, 0);
                this.StrainTime = min_strain_time;
                this.Angle = 0;
                return;
            }
            this.StrainTime = Math.Max(min_strain_time, current.StartTime - previous.EndTime);
            this.Travel = current.Position - previous.Position;
            if (previousPrevious is null)
            {
                this.Angle = 0;
                return;
            }
            double bSqr = (previousPrevious.Position - current.Position).LengthSquared();
            double c = (previous.Position - previousPrevious.Position).Length();
            if (Distance == 0 || bSqr == 0 || c == 0)
                this.Angle = 0;
            else 
                this.Angle = Math.Acos((Math.Pow(Distance, 2) + Math.Pow(c, 2) - bSqr) / (2 * Distance * c));
        }
        public static IEnumerable<DifficultyObject> CreateDifficultyObjects(List<HitObject> hitObjects)
        {
            for (int i = 0; i < hitObjects.Count(); i++)
            {
                HitObject currObj = hitObjects[i];
                HitObject? prevObj = (i == 0) ? null : hitObjects[i-1];
                HitObject? prevPrevObj = (i < 2) ? null : hitObjects[i-2];
                yield return new DifficultyObject(currObj, prevObj, prevPrevObj);
            }
        }
    }
}