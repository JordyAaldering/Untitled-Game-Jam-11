using UnityEngine;

namespace DefaultNamespace
{
    public static class ShapeGenerator
    {
        public static void Cut(ref float[] horizontal, ref float[] vertical, int width, int height)
        {
            horizontal = CutHorizontal(horizontal, width);
            vertical = CutVertical(vertical, height);
        }

        private static float[] CutHorizontal(float[] horizontal, int width)
        {
            int amount = horizontal.Length;
            if (amount == 0)
                return new float[0];
        
            float[] cuts = GetFractions(amount);
            for (int i = 0; i < amount; i++)
            {
                cuts[i] *= width;
            }

            return cuts;
        }

        private static float[] CutVertical(float[] vertical, int height)
        {
            int amount = vertical.Length;
            if (amount == 0)
                return new float[0];

            float[] cuts = GetFractions(amount);
            for (int i = 0; i < amount; i++)
            {
                cuts[i] *= height;
            }

            return cuts;
        }

        private static float[] GetFractions(int amount)
        {
            float avg = 1 / (amount + 1);
            float avgHalf = avg * 0.5f;
            
            float[] fractions = new float[amount];
            for (int i = 0; i < amount; i++)
            {
                float frac = (i + 1) / (amount + 1);
                frac += Random.Range(-avgHalf, avgHalf);
                fractions[i] = frac;
            }

            return fractions;
        }
    }
}
