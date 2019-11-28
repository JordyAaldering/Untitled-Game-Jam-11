using UnityEngine;

namespace Game
{
    public class MovingWall : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;

        private float currentMoveSpeed = 0f;
        private float endPos = 0f;

        public void Initialise(float startPos, float endPos)
        {
            transform.position = new Vector3(0f, 0f, startPos);
            currentMoveSpeed = moveSpeed;
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
