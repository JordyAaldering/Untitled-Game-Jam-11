using Board;
using Grid;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
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
            if (Input.GetKeyDown(KeyCode.P))
                StartLevel();
        }

        public void StartLevel()
        {
            generator.Generate();
            tex.SetTexture();
            wall.Initialise(10f, -10f);
        }
    }
}
