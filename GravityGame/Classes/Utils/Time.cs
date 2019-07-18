namespace GravityGame.Utils
{
    public class Time
    {
        public double TimeMultiplier;
        public float FixedDeltaTime;

        public float CurrentTime { get; set; }

        public const double FrameRate = 1d / 60d;

        public Time(float startTime = 0f, double timeMultiplier = 1d)
        {
            TimeMultiplier = timeMultiplier;
            FixedDeltaTime = (float)(TimeMultiplier * FrameRate);
            CurrentTime = startTime;
        }

        public Time Copy()
        {
            return new Time(CurrentTime, TimeMultiplier);
        }

        public void Update()
        {
            CurrentTime += FixedDeltaTime;
        }
    }
}