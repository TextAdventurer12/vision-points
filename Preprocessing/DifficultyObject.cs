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
        public Vector2 TravelVelocity => Vector2.Divide(Travel, (Single)StrainTime);
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
        /// The velocity from the previous object to this object
        /// </summary>
        public double Velocity => Distance / StrainTime;

        public HitObject BaseObject;

        private List<DifficultyObject> objects;
        public int Index;
        public DifficultyObject? Previous(int n)
        {
            // Check out of range both sides in case of negative n
            if (Index - n < 0 || Index - n > objects.Count())
                return null;
            return objects[Index - n];
        }
        public DifficultyObject? Next(int n)
        {
            // Check out of range both sides in case of negative n
            if (Index + n < 0 || Index + n > objects.Count())
                return null;
            return objects[Index + n];
        }

        public DifficultyObject(List<DifficultyObject> diffObjects, List<HitObject> baseObjects, int Index)
        {
            this.Index = Index;
            this.BaseObject = baseObjects[Index];
            this.objects = diffObjects;
            DifficultyObject? previous = Previous(1);
            if (previous is null)
            {
                this.Travel = new Vector2(0, 0);
                this.StrainTime = 0;
                this.Angle = 0;
                return;
            }
            this.StrainTime = BaseObject.StartTime - previous.BaseObject.EndTime;
            this.Travel = BaseObject.Position - previous.BaseObject.Position;
            DifficultyObject? previousPrevious = Previous(2);
            if (previousPrevious is null)
            {
                this.Angle = 0;
                return;
            }
            double bSqr = (previousPrevious.BaseObject.Position - BaseObject.Position).LengthSquared();
            double c = (previous.BaseObject.Position - previousPrevious.BaseObject.Position).Length();
            if (Distance == 0 || bSqr == 0 || c == 0)
                this.Angle = 0;
            else 
                this.Angle = Math.Acos((Math.Pow(Distance, 2) + Math.Pow(c, 2) - bSqr) / (2 * Distance * c));
        }
        public static List<DifficultyObject> CreateDifficultyObjects(List<HitObject> hitObjects)
        {
            List<DifficultyObject> objects = new List<DifficultyObject>();
            for (int i = 0; i < hitObjects.Count(); i++)
                objects.Add(new DifficultyObject(objects, hitObjects, i));
            return objects;
        }
    }
}