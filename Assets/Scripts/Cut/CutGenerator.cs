using Board;

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
            cutSettings.horizontalCutFractions = new float[amount];
            for (int i = 0; i < amount; i++)
            {
                cutSettings.horizontalCutFractions[i] = (float) (i + 1) / (amount + 1);
            }
        }
        
        private static void SetVerticalFractions(CutSettings cutSettings, int amount)
        {
            cutSettings.verticalCutFractions = new float[amount];
            for (int i = 0; i < amount; i++)
            {
                cutSettings.verticalCutFractions[i] = (float) (i + 1) / (amount + 1);
            }
        }
    }
}
