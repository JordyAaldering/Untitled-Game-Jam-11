#pragma warning disable 0649
using Board;
using UnityEngine;

namespace Game
{
    public class MovingWall : MonoBehaviour
    {
        [SerializeField] private BoardSettings boardSettings;

        private float currentMoveSpeed = 0f;
        private float endPos = 0f;

        public void Initialise(float startPos, float endPos)
        {
            transform.position = new Vector3(0f, 0f, startPos);
            currentMoveSpeed = boardSettings.wallMoveSpeed;
            this.endPos = endPos;
        }
        
        private void Update()
        {
            if (transform.position.z > endPos)
            {
                transform.Translate(Time.deltaTime * currentMoveSpeed * -transform.forward);
            }
        }
    }
}
