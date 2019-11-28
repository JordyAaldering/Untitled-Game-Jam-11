#pragma warning disable 0649
using Board;
using Grid;
using UnityEngine;

namespace UI
{
    public class MenuCanvas : MonoBehaviour
    {
        [SerializeField] private BoardSettings boardSettings;
        [SerializeField] private GridSettings gridSettings;
        public void SetDifficulty(float value)
        {
            boardSettings.difficulty = (int) value;
            gridSettings.difficulty = (int) value;
        }
    }
}
