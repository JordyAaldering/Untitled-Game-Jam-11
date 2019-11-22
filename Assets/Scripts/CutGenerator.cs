using UnityEngine;

namespace DefaultNamespace
{
    public static class CutGenerator
    {
        public static void Cut(ref float[] horizontal, ref float[] vertical, float boardWidth, float boardHeight)
        {
            horizontal = CutHorizontal(horizontal, boardWidth);
            vertical = CutVertical(vertical, boardHeight);
        }

        private static float[] CutHorizontal(float[] horizontal, float boardWidth)
        {
            int amount = horizontal.Length;
            if (amount == 0)
                return new float[0];
        
            float[] cuts = GetFractions(amount);
            for (int i = 0; i < amount; i++)
            {
                cuts[i] *= boardWidth;
            }

            return cuts;
        }

        private static float[] CutVertical(float[] vertical, float boardHeight)
        {
            int amount = vertical.Length;
            if (amount == 0)
                return new float[0];

            float[] cuts = GetFractions(amount);
            for (int i = 0; i < amount; i++)
            {
                cuts[i] *= boardHeight;
            }

            return cuts;
        }

        private static float[] GetFractions(int amount)
        {
            float avg = (float) 1 / (amount + 1);
            float avgHalf = avg * 0.5f;
            
            float[] fractions = new float[amount];
            for (int i = 0; i < amount; i++)
            {
                float frac = (float) (i + 1) / (amount + 1);
                frac += Random.Range(-avgHalf, avgHalf);
                fractions[i] = frac;
            }

            return fractions;
        }
    }
}
