using Board;
using UnityEngine;

namespace Cut
{
    public static class CutGenerator
    {
        public static void Cut(BoardSettings boardSettings, CutSettings cutSettings)
        {
            cutSettings.wallWidth = boardSettings.boardWidth;
            cutSettings.wallHeight = boardSettings.boardHeight;
            
            cutSettings.horizontalCuts = new float[boardSettings.horizontalCutAmount];
            cutSettings.verticalCuts = new float[boardSettings.verticalCutAmount];
            
            CutHorizontal(cutSettings);
            CutVertical(cutSettings);
        }

        private static void CutHorizontal(CutSettings cutSettings)
        {
            int amount = cutSettings.horizontalCuts.Length;
            if (amount == 0)
                return;

            SetHorizontalFractions(cutSettings, amount);
            for (int i = 0; i < amount; i++)
            {
                cutSettings.horizontalCuts[i] = cutSettings.horizontalCutFractions[i] * cutSettings.wallWidth;
            }
        }

        private static void CutVertical(CutSettings cutSettings)
        {
            int amount = cutSettings.verticalCuts.Length;
            if (amount == 0)
                return;

            SetVerticalFractions(cutSettings, amount);
            for (int i = 0; i < amount; i++)
            {
                cutSettings.verticalCuts[i] = cutSettings.verticalCutFractions[i] * cutSettings.wallHeight;
            }
        }

        private static void SetHorizontalFractions(CutSettings cutSettings, int amount)
        {
            float avg = (float) 1 / (amount + 1);
            float maxOffset = avg * 0.5f * cutSettings.maxCutOffset;

            cutSettings.horizontalCutFractions = new float[amount];
            for (int i = 0; i < amount; i++)
            {
                float frac = (float) (i + 1) / (amount + 1);
                frac += Random.Range(-maxOffset, maxOffset);
                cutSettings.horizontalCutFractions[i] = frac;
            }
        }
        
        private static void SetVerticalFractions(CutSettings cutSettings, int amount)
        {
            float avg = (float) 1 / (amount + 1);
            float maxOffset = avg * 0.5f * cutSettings.maxCutOffset;

            cutSettings.verticalCutFractions = new float[amount];
            for (int i = 0; i < amount; i++)
            {
                float frac = (float) (i + 1) / (amount + 1);
                frac += Random.Range(-maxOffset, maxOffset);
                cutSettings.verticalCutFractions[i] = frac;
            }
        }
    }
}
