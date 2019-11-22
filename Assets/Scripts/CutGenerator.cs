using UnityEngine;

namespace DefaultNamespace
{
    public static class CutGenerator
    {
        public static void Cut(ref float[] horizontal, ref float[] vertical, CutSettings settings)
        {
            horizontal = CutHorizontal(horizontal, settings);
            vertical = CutVertical(vertical, settings);
        }

        private static float[] CutHorizontal(float[] horizontal, CutSettings settings)
        {
            int amount = horizontal.Length;
            if (amount == 0)
                return new float[0];
        
            float[] cuts = GetFractions(amount, settings);
            for (int i = 0; i < amount; i++)
            {
                cuts[i] *= settings.width;
            }

            return cuts;
        }

        private static float[] CutVertical(float[] vertical, CutSettings settings)
        {
            int amount = vertical.Length;
            if (amount == 0)
                return new float[0];

            float[] cuts = GetFractions(amount, settings);
            for (int i = 0; i < amount; i++)
            {
                cuts[i] *= settings.height;
            }

            return cuts;
        }

        private static float[] GetFractions(int amount, CutSettings settings)
        {
            float avg = (float) 1 / (amount + 1);
            float maxOffset = avg * 0.5f * settings.maxCutOffset;
            
            float[] fractions = new float[amount];
            for (int i = 0; i < amount; i++)
            {
                float frac = (float) (i + 1) / (amount + 1);
                frac += Random.Range(-maxOffset, maxOffset);
                fractions[i] = frac;
            }

            return fractions;
        }
    }

    [CreateAssetMenu(menuName="Game Settings/Cut Settings", fileName="New Cut Settings")]
    public class CutSettings : ScriptableObject
    {
        [HideInInspector] public float width = 0;
        [HideInInspector] public float height = 0;
        
        [Range(0f, 1f)] public float maxCutOffset = 0.5f;
    }
}
