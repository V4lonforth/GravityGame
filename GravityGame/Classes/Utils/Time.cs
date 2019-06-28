namespace GravityGame.Utils
{
    public static class Time
    {
        public static double TimeMultiplier = 1.75d;
        public static float FixedDeltaTime = (float)(FrameRate * TimeMultiplier);

        public static float CurrentTime { get; set; }

        public const double FrameRate = 1d / 60d;
        public const double SecondsInDay = 24d * 3600d;

        public static void Update()
        {
            CurrentTime += FixedDeltaTime;
        }
    }
}