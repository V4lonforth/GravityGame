using System;

namespace GravityGame.Utils
{
    public class FloatRandom
    {
        private float[] randomValues;
        private int randomIndex;

        private const int randomValuesCount = 1737;

        public FloatRandom()
        {
            Random random = new Random();
            randomValues = new float[randomValuesCount];
            for (int i = 0; i < randomValuesCount; i++)
                randomValues[i] = (float)random.NextDouble();
        }

        public float NextFloat(float min, float max)
        {
            return NextFloat() * (max - min) + min;
        }

        public float NextFloat(float max)
        {
            return NextFloat() * max;
        }

        public float NextFloat()
        {
            randomIndex = (randomIndex + 1) % randomValuesCount;
            return randomValues[randomIndex];
        }
    }
}