using Board;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private BoardGenerator generator;
        private MovingWall wall;

        private void Awake()
        {
            generator = FindObjectOfType<BoardGenerator>();
            wall = FindObjectOfType<MovingWall>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
                StartLevel();
        }

        private void StartLevel()
        {
            generator.Generate();
            wall.Initialise(10f, -5f);
        }
    }
}
