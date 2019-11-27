using Board;

namespace Cut
{
    public static class CutGenerator
    {
        public static void Cut(BoardSettings boardSettings, CutSettings cutSettings)
        {
            cutSettings.horizontalCuts = new float[boardSettings.horizontalCutAmount];
            cutSettings.verticalCuts = new float[boardSettings.verticalCutAmount];
            
            CutHorizontal(boardSettings, cutSettings);
            CutVertical(boardSettings, cutSettings);
        }

        private static void CutHorizontal(BoardSettings boardSettings, CutSettings cutSettings)
        {
            int amount = cutSettings.horizontalCuts.Length;
            if (amount == 0)
                return;

            float[] fractions = GetFractions(amount);
            for (int i = 0; i < amount; i++)
            {
                cutSettings.horizontalCuts[i] = fractions[i] * boardSettings.boardWidth;
            }
        }

        private static void CutVertical(BoardSettings boardSettings, CutSettings cutSettings)
        {
            int amount = cutSettings.verticalCuts.Length;
            if (amount == 0)
                return;

            float[] fractions = GetFractions(amount);
            for (int i = 0; i < amount; i++)
            {
                cutSettings.verticalCuts[i] = fractions[i] * boardSettings.boardHeight;
            }
        }

        private static float[] GetFractions(int amount)
        {
            float[] fractions = new float[amount];
            for (int i = 0; i < amount; i++)
                fractions[i] = (float) (i + 1) / (amount + 1);
            return fractions;
        }
    }
}
