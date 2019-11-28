#pragma warning disable 0649
using Board;
using Grid;
using UI;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float startPos = 10f;
        private bool isPlaying = false;

        [Header("Settings")]
        [SerializeField] private BoardSettings boardSettings;
        [SerializeField] private GridSettings gridSettings;
        [SerializeField] private GridCurrent gridCurrent;

        [Header("References")]
        [SerializeField] private GameObject menuCanvasObj;
        [SerializeField] private GameObject gameCanvasObj;
        private GameCanvas _gameCanvas;
        private GameCanvas gameCanvas
        {
            get
            {
                if (!_gameCanvas)
                    _gameCanvas = gameCanvasObj.GetComponent<GameCanvas>();
                return _gameCanvas;
            }
        }
        
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
        
        private SetGridTexture _gridTex;
        private SetGridTexture gridTex
        {
            get
            {
                if (!_gridTex)
                    _gridTex = FindObjectOfType<SetGridTexture>();
                return _gridTex;
            }
        }
        
        private SetQuadTexture _quadTex;
        private SetQuadTexture quadTex
        {
            get
            {
                if (!_quadTex)
                    _quadTex = FindObjectOfType<SetQuadTexture>();
                return _quadTex;
            }
        }

        private void Update()
        {
            Time.timeScale = isPlaying && Input.GetKey(KeyCode.W) ? 10f : 1f;
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartLevel();
            }
            else if (isPlaying && wall.transform.position.z <= boardSettings.boardDepth)
            {
                isPlaying = false;
                menuCanvasObj.SetActive(true);
                gameCanvas.SetEndText(gridCurrent.CheckSolution(gridSettings));
            }
        }

        public void StartLevel()
        {
            isPlaying = true;
            menuCanvasObj.SetActive(false);
            gameCanvas.endText.gameObject.SetActive(false);
            
            generator.Generate();
            gridTex.SetTexture();
            quadTex.SetTexture();
            wall.Initialise(startPos, boardSettings.boardDepth * 0.5f);
        }
    }
}
