using Board;
using Grid;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float startPos = 10f;
        private bool isPlaying = false;

        [SerializeField] private BoardSettings boardSettings;
        [SerializeField] private GridSettings gridSettings;
        [SerializeField] private GridCurrent gridCurrent;
        
        private BoardGenerator _generator;
        private BoardGenerator generator
        {
            get
            {
                if (!_generator)
                    _generator = FindObjectOfType<BoardGenerator>();
                return _generator;
            }
        }
        
        private MovingWall _wall;
        private MovingWall wall
        {
            get
            {
                if (!_wall)
                    _wall = FindObjectOfType<MovingWall>();
                return _wall;
            }
        }
        
        private SetGridTexture _tex;
        private SetGridTexture tex
        {
            get
            {
                if (!_tex)
                    _tex = FindObjectOfType<SetGridTexture>();
                return _tex;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartLevel();
            
            if (isPlaying && wall.transform.position.z <= boardSettings.boardDepth)
            {
                Debug.Log(gridCurrent.CheckSolution(gridSettings));
                isPlaying = false;
            }
        }

        public void StartLevel()
        {
            generator.Generate();
            tex.SetTexture();
            wall.Initialise(startPos, boardSettings.boardDepth * 0.5f);
            isPlaying = true;
        }
    }
}
