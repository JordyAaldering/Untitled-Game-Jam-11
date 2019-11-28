#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameCanvas : MonoBehaviour
    {
        public Text endText;

        public void SetEndText(bool win)
        {
            endText.text = win ? "You win!" : "You Lose!";
            endText.gameObject.SetActive(true);
        }
    }
}
